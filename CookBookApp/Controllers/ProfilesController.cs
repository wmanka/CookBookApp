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
        private readonly ApplicationUserManager userManager;
        private readonly ApplicationDbContext context;

        public ProfilesController(ApplicationUserManager userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public IActionResult Details(string id)
        {
            var user = userManager.Users
                .Include(u => u.Recipes).Include(u => u.Files)
                .Include(u => u.FavouriteRecipes)
                .ThenInclude(fr => fr.Recipe)
                .FirstOrDefault(u => u.Id == id);

            var picture = context.ProfilePictures.Where(p => p.UserId == user.Id)
                    .FirstOrDefault(f => f.FileType == FileType.Avatar);

            var vm = new ProfileDetailsViewModel()
            {
                User = user,
                MealCategories = context.Categories.ToList(),
            };

            if (picture != null)
                ViewData["AvatarPath"] = "data:image/jpeg;base64," + Convert.ToBase64String(picture.Content, 0, picture.Content.Length);

            return View("Details", vm);
        }
    }
}