using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Models.ViewModels;
using CookBookApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookBookApp.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationUserManager UserManager;
        private readonly ApplicationDbContext Context;
        private readonly IProfilePictureService ProfilePictureService;

        public ProfilesController(ApplicationUserManager userManager,
            ApplicationDbContext context, IProfilePictureService profilePictureService)
        {
            UserManager = userManager;
            Context = context;
            ProfilePictureService = profilePictureService;
        }

        public IActionResult Details(string id)
        {
            var user = UserManager.Users
                .Include(u => u.Recipes).Include(u => u.Files)
                .Include(u => u.FavouriteRecipes)
                .ThenInclude(fr => fr.Recipe)
                .FirstOrDefault(u => u.Id == id);

            var profilePicture = ProfilePictureService.GetUserAvatar(user.Id);

            var path = ProfilePictureService.GetAvatarPath(profilePicture);

            if (path != null)
                ViewData["AvatarPath"] = path;

            var vm = new ProfileDetailsViewModel()
            {
                User = user,
                MealCategories = Context.Categories.ToList(),
            };

            return View("Details", vm);
        }
    }
}