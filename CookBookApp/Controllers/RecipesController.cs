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

namespace CookBookApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public RecipesController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index(string category)
        {
            IEnumerable<Recipe> recipes = context.Recipes.Include(r => r.Category).Include(r => r.Picture).OrderByDescending(r => r.CreatedAt);

            if (!(category == "recentlyAdded" || category == null))
            { 
                recipes = recipes.Where(r => r.Category.Name == category);
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

            CreateOrUpdateRecipe(recipe);
            AddIngredientsToRecipe(vm.ChosenIngredients, recipe.Id);

            return Json(recipe.Id);
        }

        public IActionResult Details(int id)
        {
            var recipe = context.Recipes
                .Include(r => r.Category).Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);

            if (recipe == null) return NotFound();

            var ingredients = context.IngredientsInRecipes
                .Include(i => i.Ingredient)
                .Where(i => i.RecipeId == id)
                .Select(ingredient => new IngredientWithQuantity()
                {
                    Ingredient = ingredient.Ingredient,
                    Quantity = ingredient.Quantity
                });

            var recipePicture = context.RecipePictures.FirstOrDefault(rp => rp.RecipeId == recipe.Id);

            if (recipePicture != null)
            {
                var path = "data:image/jpeg;base64," +
                    Convert.ToBase64String(recipePicture.Content, 0, recipePicture.Content.Length);
                    ViewData["RecipePicturePath"] = path;
            }

            var vm = new RecipeDetailsViewModel()
            {
                Recipe = recipe,
                Ingredients = ingredients,
                IsFavouritedByCurrentUser = CheckIfFavouritedByCurrentUser(recipe),
                NumberOfLikes = context.FavouriteRecipes.Where(fr => fr.RecipeId == id).Count()
            };

            return View(vm);
        }

        [Authorize]
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

        [Authorize]
        public IActionResult Delete(int id)
        {
            var recipe = context.Recipes.FirstOrDefault(r => r.Id == id);

            context.Recipes.Remove(recipe);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // PRIVATE METHODS

        private void AddIngredientsToRecipe(IEnumerable<IngredientInRecipe> ingredients, int recipeId)
        {
            foreach (var ingredient in ingredients)
            {
                var newIngredient = new IngredientInRecipe()
                {
                    IngredientId = ingredient.IngredientId,
                    Quantity = ingredient.Quantity,
                    RecipeId = recipeId
                };

                context.IngredientsInRecipes.Add(newIngredient);
            }

            context.SaveChanges();
        }

        private void CreateOrUpdateRecipe(Recipe recipe)
        {
            if (recipe.Id == 0)
            {
                context.Recipes.Add(recipe);
            }
            else
            {
                context.Recipes.Update(recipe);

                var currentIngredients = context.IngredientsInRecipes.Where(i => i.RecipeId == recipe.Id);

                foreach (var ingredient in currentIngredients)
                {
                    context.IngredientsInRecipes.Remove(ingredient);
                }
            }

            context.SaveChanges();
        }

        private bool CheckIfFavouritedByCurrentUser(Recipe recipe)
        {
            var currentUserId = userManager.GetUserId(User);

            var favouritedRecipes = context.FavouriteRecipes
                .FirstOrDefault(fr => fr.RecipeId == recipe.Id && fr.UserId == currentUserId);

            if (favouritedRecipes != null) return true;
            else return false;
        }
    }
}