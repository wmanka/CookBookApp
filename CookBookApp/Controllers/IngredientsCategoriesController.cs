using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.DTO;
using CookBookApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApp.Controllers
{
    [Route("api/ingredientscategories")]
    public class IngredientsCategoriesController : Controller
    {
        private readonly ApplicationDbContext context;

        public IngredientsCategoriesController(ApplicationDbContext context) => this.context = context;

        [HttpGet]
        // GET: api/ingredientscategories
        public IActionResult Get()
        {
            var ingredientsCategories = context.IngredientCategory.OrderByDescending(i => i.Name).ToList();

            return Ok(ingredientsCategories);
        }

        // GET api/ingredientscategories/{id}
        [HttpGet("{id}", Name = "GetIngredientCategory")]
        public IActionResult Get(int id)
        {
            var result = context.IngredientCategory.FirstOrDefault(ic => ic.Id == id);

            if (result == null) return NotFound();

            return Ok(result);
        }

        // POST api/ingredientscategories
        [HttpPost]
        public IActionResult Post(IngredientCategoryDTO ingredientCategory)
        {
            if (ingredientCategory == null) return BadRequest();

            var finalCategory = new IngredientCategory()
            {
                Name = ingredientCategory.Name
            };

            context.IngredientCategory.Add(finalCategory);
            context.SaveChanges();

            return CreatedAtRoute("GetIngredientCategory", new { id = finalCategory.Id }, finalCategory);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = context.IngredientCategory.FirstOrDefault(ic => ic.Id == id);

            if (category == null)
                return NotFound();

            context.IngredientCategory.Remove(category);
            context.SaveChanges();

            return NoContent();
        }
    }
}