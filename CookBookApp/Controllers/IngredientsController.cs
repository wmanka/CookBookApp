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
        private readonly ApplicationDbContext context;

        public IngredientsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/ingredients
        [HttpGet]
        public IActionResult Get()
        {
            var ingredients = context.Ingredients.OrderByDescending(i => i.Name).Include(i => i.Category).ToList();

            return Ok(ingredients);
        }

        // GET api/ingredients/5
        [HttpGet("{id}", Name = "GetIngredient")]
        public IActionResult Get(int id)
        {
            var ingredient = context.Ingredients.FirstOrDefault(i => i.Id == id);

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

            context.Ingredients.Add(finalIngredient);
            context.SaveChanges();
            return CreatedAtRoute("GetIngredient", new { id = finalIngredient.Id }, finalIngredient); // Set Custom Name Parameter for HttpGet!
        }

        //PUT api/ingredients/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, IngredientDTO ingredient)
        {
            if (ingredient == null) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ingredientFromDb = context.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredientFromDb == null) return NotFound();

            ingredientFromDb.Name = ingredient.Name;
            ingredientFromDb.CategoryId = ingredient.CategoryId;
            ingredientFromDb.Description = ingredient.Description;

            context.Ingredients.Update(ingredientFromDb);
            context.SaveChanges();

            return NoContent();
        }

        // DELETE api/ingredients/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ingredient = context.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredient == null)
                return NotFound();

            context.Ingredients.Remove(ingredient);
            context.SaveChanges();

            return NoContent();
        }
    }
}
