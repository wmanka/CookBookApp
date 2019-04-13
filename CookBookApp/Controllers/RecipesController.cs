using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            recipe.CreatedAt = DateTime.Now;
            recipe.UserId = userManager.GetUserId(User);

            context.Recipes.Add(recipe);
            context.SaveChanges();

            var id = recipe.Id;

            foreach (var ingredient in vm.ChosenIngredients)
            {
                var ing = new IngredientInRecipe()
                {
                    IngredientId = ingredient.IngredientId,
                    Quantity = ingredient.Quantity,
                    RecipeId = id
                };

                context.IngredientsInRecipes.Add(ing);
            }

            context.SaveChanges();

            return Json(new { redirectToUrl = Url.Action("Index", "Home") });
        }

        public IActionResult Details(int id)
        {
            var ingredientsInRecipe = context.IngredientsInRecipes.Where(i => i.RecipeId == id).ToList();

            var list = new List<IngredientWithQuantity>();

            foreach (var ingredient in ingredientsInRecipe)
            {
                var item = new IngredientWithQuantity()
                {
                    Ingredient = context.Ingredients.FirstOrDefault(i => i.Id == ingredient.IngredientId),
                    Quantity = ingredient.Quantity
                };

                list.Add(item);
            }

            var vm = new RecipeDetailsViewModel()
            {
                Recipe = context.Recipes.Include(r => r.Category).Include(r => r.User).FirstOrDefault(r => r.Id == id),
                Ingredients = list
            };

            return View(vm);
        }

    }
}