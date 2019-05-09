using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.DTO;
using CookBookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApp.Controllers.API
{
    [Route("api/favouriterecipes")]
    public class FavouriteRecipes : Controller
    {
        public ApplicationDbContext Context { get; set; }
        public FavouriteRecipes(ApplicationDbContext context) => Context = context;

        // GET: api/favouriterecipes
        [HttpGet]
        public IActionResult Get()
        {
            var fr = Context.FavouriteRecipes.OrderByDescending(fr => fr.CreatedAt).ToList();
            return Ok(fr);
        }

        // POST api/favouriterecipes
        [HttpPost]
        public IActionResult Post(FavouriteRecipe favouriteRecipe)
        {
            if (favouriteRecipe == null) BadRequest();

            var frFromDb = Context.FavouriteRecipes.
                FirstOrDefault(fr => fr.RecipeId == favouriteRecipe.RecipeId && fr.UserId == favouriteRecipe.UserId);

            if (frFromDb == null)
            {
                favouriteRecipe.CreatedAt = DateTime.Now;
                Context.FavouriteRecipes.Add(favouriteRecipe);
                Context.SaveChanges();
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return Json("Error - item is already marked as favourite");

            }
        }

        // DELETE api/favouriterecipes/a6af761b-3f53-4bc0-a7a4-99fa86ef5d51/5
        [HttpDelete("{UserId}/{RecipeId}")]
        public void Delete(string UserId, int RecipeId)
        {
            if (UserId == null) BadRequest();

            var recipe = Context.FavouriteRecipes
                .FirstOrDefault(fr => fr.UserId == UserId && fr.RecipeId == RecipeId);

            Context.FavouriteRecipes.Remove(recipe);
            Context.SaveChanges();
        }
    }
}
