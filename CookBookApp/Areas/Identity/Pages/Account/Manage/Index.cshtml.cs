using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Services;
using CookBookApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage;

namespace CookBookApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly ApplicationUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IProfilePictureService _profilePictureService;

        public IndexModel(
            ApplicationUserManager userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IProfilePictureService profilePictureService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _profilePictureService = profilePictureService;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public string Name { get; set; }

            public string Location { get; set; }

            public Gender Gender { get; set; }

            public string Description { get; set; }

            public ProfilePicture File { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var name = await _userManager.GetNameAsync(user);
            var location = await _userManager.GetLocationAsync(user);
            var gender = await _userManager.GetGenderAsync(user);
            var description = await _userManager.GetDescriptionAsync(user);
            var picture = _profilePictureService.GetUserAvatar(user.Id);

            var path = _profilePictureService.GetAvatarPath(picture);

            if (path != null)
                ViewData["AvatarPath"] = path;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                Name = name,
                Location = location,
                Gender = gender,
                Description = description,
                File = picture
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile upload)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            var name = await _userManager.GetNameAsync(user);
            if (Input.Name != name)
            {
                var setNameResult = await _userManager.SetNameAsync(user, Input.Name);
                if (!setNameResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting name for user with ID '{userId}'.");
                }
            }

            var description = await _userManager.GetDescriptionAsync(user);
            if (Input.Description != description)
            {
                var setDescriptionResult = await _userManager.SetDescriptionAsync(user, Input.Description);
                if (!setDescriptionResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting description for user with ID '{userId}'.");
                }
            }

            var location = await _userManager.GetLocationAsync(user);
            if (Input.Location != location)
            {
                var setLocationResult = await _userManager.SetLocationAsync(user, Input.Location);
                if (!setLocationResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting location for user with ID '{userId}'.");
                }
            }

            var gender = await _userManager.GetGenderAsync(user);
            if (Input.Gender != gender)
            {
                var setGenderResult = await _userManager.SetGenderAsync(user, Input.Gender);
                if (!setGenderResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting location for user with ID '{userId}'.");
                }
            }

            var currentAvatar = _profilePictureService.GetUserAvatar(user.Id);
            try
            {
                if (upload != null && upload.Length > 0)
                {
                    var avatar = new ProfilePicture
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.Avatar,
                        ContentType = upload.ContentType,
                        UserId = user.Id
                    };

                    using (var reader = new System.IO.BinaryReader(upload.OpenReadStream()))
                    {
                        avatar.Content = reader.ReadBytes((int)upload.Length);
                    }

                    if(currentAvatar != null)
                        _profilePictureService.Remove(currentAvatar);

                    _profilePictureService.Add(avatar);
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }  

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
