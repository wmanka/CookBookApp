using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using CookBookApp.Services.Interfaces;

namespace CookBookApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRecipeService recipeService;

        public RecipesController(ApplicationDbContext context, 
            IRecipeService recipeService,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.recipeService = recipeService;
        }

        public IActionResult Index(string category)
        {
            var recipes = recipeService.GetRecipes();

            if (!(category == "recentlyAdded" || category == null))
            { 
                recipes = recipeService.GetRecipes(r => r.Category.Name == category);
            }

            var vm = new RecipesIndexViewModel()
            {
                Recipes = recipes.ToList(),
                Categories = context.Categories.ToList(),
                RecipePictures = context.RecipePictures.ToList()
            };

            return View(vm);
        }

        [Authorize]
        public IActionResult Create()
        {
            var vm = new CreateRecipeViewModel()
            {
                MealCategories = context.Categories.OrderBy(c => c.Name).ToList(),
                Ingredients = context.Ingredients.OrderBy(i => i.Name).ToList(),
                Recipe = new Recipe(),
                ChosenIngredients = new List<IngredientInRecipe>()
            };

            return View(vm);
        }

        [Authorize]
        public IActionResult CreateRecipe(CreateRecipeViewModel vm)
        {
            var recipe = vm.Recipe;

            recipe.CreatedAt = DateTime.Now;
            recipe.UserId = userManager.GetUserId(User);

            recipeService.CreateOrUpdateRecipe(recipe);
            recipeService.AddIngredientsToRecipe(vm.ChosenIngredients, recipe.Id);

            return Json(recipe.Id);
        }

        public IActionResult Details(int id)
        {
            var recipe = recipeService.GetRecipe(id);

            if (recipe == null) return NotFound();

            var ingredients = context.IngredientsInRecipes
                .Include(i => i.Ingredient)
                .Where(i => i.RecipeId == id)
                .Select(ingredient => new IngredientWithQuantity()
                {
                    Ingredient = ingredient.Ingredient,
                    Quantity = ingredient.Quantity
                });

            var recipePicture = recipeService.GetRecipePicture(recipe.Id);

            if (recipePicture != null)
            {
                ViewData["RecipePicturePath"] = recipeService.GetPicturePath(recipePicture);
            }

            var currentUserId = userManager.GetUserId(User);

            var vm = new RecipeDetailsViewModel()
            {
                Recipe = recipe,
                Ingredients = ingredients,
                IsFavouritedByCurrentUser = recipeService.CheckIfRecipeIsFavouritedByUser(recipe, currentUserId),
                NumberOfLikes = recipeService.GetNumberOfLikes(id)
            };

            return View(vm);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var vm = new CreateRecipeViewModel()
            {
                Ingredients = context.Ingredients.ToList(),
                Recipe = recipeService.GetRecipe(id),
                MealCategories = context.Categories.ToList(),
                ChosenIngredients = context.IngredientsInRecipes.Where(i => i.RecipeId == id),
                MealCategoryId = context.Recipes.FirstOrDefault(r => r.Id == id).Category.Id
            };

            return View("Create", vm);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            recipeService.Remove(id);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreatePdf(int id)
        {
            var recipePdf = recipeService.GetPdf(id);
            return File(recipePdf, "application/pdf");
        }
    }
}