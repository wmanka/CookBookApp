using System;
using System.Collections.Generic;
using System.Linq;
using CookBookApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Services;

namespace CookBookApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleTaskListsController : ControllerBase
    {
        private readonly IGoogleTasksService GoogleTasksService;
        private readonly IRecipeService RecipesService;

        public GoogleTaskListsController(IGoogleTasksService taskService, IRecipeService recipeService)
        {
            GoogleTasksService = taskService;
            RecipesService = recipeService;
        }

        [HttpPost]
        public IActionResult Post(int id)
        {
            var recipe = RecipesService.GetRecipe(id);

            if (recipe == null) return NotFound();

            var ingredients = recipe.Ingredients.OrderBy(i => i.Ingredient.Name).ToList();
            var currentTaskList = GoogleTasksService.GetTaskList("Shopping List - " + recipe.Name);

            if (currentTaskList != null) return Conflict();

            GoogleTasksService.AddTaskList(recipe.Name);
            var taskList = GoogleTasksService.GetTaskList("Shopping List - " + recipe.Name);

            foreach (var ingredient in ingredients)
            {
                var task = new Task { Title = ingredient.Ingredient.Name + ": " + ingredient.Quantity };
                GoogleTasksService.AddTask(taskList, task);
            }

            return Ok();
        }
    }
}