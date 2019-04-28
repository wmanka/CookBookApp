using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookBookApp.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationUserManager UserManager;
        private readonly ApplicationDbContext Context;

        public ProfilesController(ApplicationUserManager userManager,
            ApplicationDbContext context)
        {
            UserManager = userManager;
            Context = context;
        }

        public IActionResult Details(string id)
        {
            var user = UserManager.Users
                .Include(u => u.Recipes).Include(u => u.Files)
                .Include(u => u.FavouriteRecipes)
                .ThenInclude(fr => fr.Recipe)
                .FirstOrDefault(u => u.Id == id);

            var picture = Context.ProfilePictures.Where(p => p.UserId == user.Id)
                    .FirstOrDefault(f => f.FileType == FileType.Avatar);

            var vm = new ProfileDetailsViewModel()
            {
                User = user,
                MealCategories = Context.Categories.ToList(),
            };

            if (picture != null)
                ViewData["AvatarPath"] = "data:image/jpeg;base64," + Convert.ToBase64String(picture.Content, 0, picture.Content.Length);

            return View("Details", vm);
        }
    }
}