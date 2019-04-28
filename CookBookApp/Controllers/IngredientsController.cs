using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.DTO;
using CookBookApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookBookApp.Controllers
{
    [Route("api/ingredients")]
    public class IngredientsController : Controller
    {
        private readonly ApplicationDbContext Context;

        public IngredientsController(ApplicationDbContext context) => Context = context;

        // GET: api/ingredients
        [HttpGet]
        public IActionResult Get()
        {
            var ingredients = Context.Ingredients.OrderByDescending(i => i.Name).Include(i => i.Category).ToList();

            return Ok(ingredients);
        }

        // GET api/ingredients/5
        [HttpGet("{id}", Name = "GetIngredient")]
        public IActionResult Get(int id)
        {
            var ingredient = Context.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredient == null)
                return NotFound();

            return Ok(ingredient);
        }

        // POST api/ingredients
        [HttpPost]
        public IActionResult Post(IngredientDTO ingredient)
        {
            if (ingredient == null) return BadRequest();

            var finalIngredient = new Ingredient()
            {
                Name = ingredient.Name,
                Description = ingredient.Description,
                CategoryId = ingredient.CategoryId,
                Category = ingredient.Category
            };

            Context.Ingredients.Add(finalIngredient);
            Context.SaveChanges();

            return CreatedAtRoute("GetIngredient", new { id = finalIngredient.Id }, finalIngredient);
        }

        //PUT api/ingredients/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, IngredientDTO ingredient)
        {
            if (ingredient == null) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ingredientFromDb = Context.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredientFromDb == null) return NotFound();

            ingredientFromDb.Name = ingredient.Name;
            ingredientFromDb.CategoryId = ingredient.CategoryId;
            ingredientFromDb.Description = ingredient.Description;

            Context.Ingredients.Update(ingredientFromDb);
            Context.SaveChanges();

            return NoContent();
        }

        // DELETE api/ingredients/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ingredient = Context.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredient == null) return NotFound();

            Context.Ingredients.Remove(ingredient);
            Context.SaveChanges();

            return NoContent();
        }
    }
}
