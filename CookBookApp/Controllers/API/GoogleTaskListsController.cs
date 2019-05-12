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

        [HttpGet]
        public IActionResult Get()
        {
            var result = GoogleTasksService.GetTaskLists();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(int id)
        {
            var recipe = RecipesService.GetRecipe(id);
            var ingredients = recipe.Ingredients.ToList();

            var currentTaskList = GoogleTasksService.GetTaskList(tl => tl.Title == "Shopping List - " + recipe.Name);

            if (currentTaskList == null)
            {
                GoogleTasksService.AddTaskList(recipe.Name);
                var taskList = GoogleTasksService.GetTaskLists().Last();

                foreach (var item in ingredients)
                {
                    var task = new Task { Title = item.Ingredient.Name + ": " + item.Quantity };
                    GoogleTasksService.AddTask(taskList, task);
                }
            }

            return Ok();
        }
    }
}