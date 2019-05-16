using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBookApp.DTO;
using CookBookApp.Models;
using CookBookApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApp.Controllers.API
{
    [Route("api/[controller]")]
    public class RecipeEmailSenderController : Controller
    {
        private readonly IEmailSender EmailSender;
        private readonly IRecipeService RecipeService;

        public RecipeEmailSenderController(IEmailSender emailSender, IRecipeService recipeService)
        {
            EmailSender = emailSender;
            RecipeService = recipeService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(RecipeEmailSenderDTO vm)
        {
            var recipe = RecipeService.GetRecipe(vm.RecipeId);

            if (recipe == null) return NotFound();
            
            await EmailSender.SendEmailAsync(
                vm.Email, 
                "Recipe: " + recipe.Name, 
                CreateRecipeEmailMessage(recipe));

            return Ok();
        }

        private string CreateRecipeEmailMessage(Recipe recipe)
        {
            var message = new StringBuilder()
                .Append($"<h1>Recipe - { recipe.Name}</h1>")
                .Append($"<p>{ recipe.ShortDescription}</p>")
                .Append($"<p>Preparation Time: { recipe.PreparationTime }</p>")
                .Append($"<h3>Ingredients</h3><ul>");

            foreach (var ingredient in recipe.Ingredients.ToList())
            {
                message.AppendLine($"<li> { ingredient.Ingredient.Name }: {ingredient.Quantity } </li>");
            }

            message.Append("</ul>")
                .Append($"<p> { recipe.Instructions } </p>")
                .Append("For more details go to <a href='https://localhost:44366/Recipes/Details/" + recipe.Id + "'>recipe page</a>");

            return message.ToString();
        }
    }
}