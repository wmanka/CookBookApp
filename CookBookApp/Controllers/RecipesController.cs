using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CookBookApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public RecipesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index(string category)
        {
            IEnumerable<Recipe> recipes = null;


            if(category == "recentlyAdded" || category == null)
            {
                recipes = context.Recipes.Include(r => r.Category).OrderByDescending(r => r.CreatedAt);
            }
            else
                recipes = context.Recipes.Include(r => r.Category).Where(r => r.Category.Name == category);

            var vm = new RecipesIndexViewModel()
            {
                Recipes = recipes.ToList(),
                Categories = context.Categories.ToList()
            };

            return View(vm);
        }

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

        public IActionResult CreateRecipe(CreateRecipeViewModel vm)
        {
            var recipe = vm.Recipe;

            if (vm.Recipe.Id == 0)
            {
                recipe.CreatedAt = DateTime.Now;
                recipe.UserId = userManager.GetUserId(User);

                context.Recipes.Add(recipe);
                context.SaveChanges();   
            }
            else
            {
                context.Recipes.Update(recipe);

                var currentIngredients = context.IngredientsInRecipes.Where(i => i.RecipeId == recipe.Id);
                foreach (var item in currentIngredients)
                {
                    context.IngredientsInRecipes.Remove(item);
                }

                context.SaveChanges();
            }

            foreach (var ingredient in vm.ChosenIngredients)
            {
                var newIngredient = new IngredientInRecipe()
                {
                    IngredientId = ingredient.IngredientId,
                    Quantity = ingredient.Quantity,
                    RecipeId = recipe.Id
                };

                context.IngredientsInRecipes.Add(newIngredient);
            }

            context.SaveChanges();

            return Json(new { redirectToUrl = Url.Action("Index", "Home") });
        }

        public IActionResult Details(int id)
        {
            var recipe = context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .Where(r => r.Id == id)
                .FirstOrDefault();

            if (recipe == null)
            {
                return NotFound();
            }

            var ingredients = context.IngredientsInRecipes
                .Include(i => i.Ingredient)
                .Where(i => i.RecipeId == id)
                .Select(ingredient => new IngredientWithQuantity()
                {
                    Ingredient = ingredient.Ingredient,
                    Quantity = ingredient.Quantity
                });

            var isFavouritedByCurrentUser = false;

            var currentUserId = userManager.GetUserId(User);
            var favouritedRecipes = context.FavouriteRecipes
                .FirstOrDefault(fr => fr.RecipeId == recipe.Id && fr.UserId == currentUserId);

            if (favouritedRecipes != null)
            {
                isFavouritedByCurrentUser = true;
            }

            var vm = new RecipeDetailsViewModel()
            {
                Recipe = context.Recipes.Include(r => r.Category).Include(r => r.User).FirstOrDefault(r => r.Id == id),
                Ingredients = ingredients,
                IsFavouritedByCurrentUser = isFavouritedByCurrentUser
            };

            return View(vm);
        }

        public IActionResult Edit(int id)
        {
            var vm = new CreateRecipeViewModel()
            {
                Ingredients = context.Ingredients.ToList(),
                Recipe = context.Recipes.FirstOrDefault(r => r.Id == id),
                MealCategories = context.Categories.ToList(),
                ChosenIngredients = context.IngredientsInRecipes.Where(i => i.RecipeId == id),
                MealCategoryId = context.Recipes.FirstOrDefault(r => r.Id == id).Category.Id
            };

            return View("Create", vm);
        }

        public IActionResult Delete(int id)
        {
            var recipe = context.Recipes.FirstOrDefault(r => r.Id == id);
            context.Recipes.Remove(recipe);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}