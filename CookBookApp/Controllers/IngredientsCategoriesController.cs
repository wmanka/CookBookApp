using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.DTO;
using CookBookApp.Models;
using CookBookApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApp.Controllers
{
    [Route("api/ingredientscategories")]
    public class IngredientsCategoriesController : Controller
    {
        private readonly IIngredientCategoryService IngredientCategoryService;

        public IngredientsCategoriesController(IIngredientCategoryService ingredientCategoryService) =>
            IngredientCategoryService = ingredientCategoryService;        

        [HttpGet]
        // GET: api/ingredientscategories
        public IActionResult Get()
        {
            var ingredientsCategories = IngredientCategoryService.GetIngredientCategories();

            return Ok(ingredientsCategories);
        }

        // GET api/ingredientscategories/{id}
        [HttpGet("{id}", Name = "GetIngredientCategory")]
        public IActionResult Get(int id)
        {
            var ingredientCategory = IngredientCategoryService.GetIngredientCategory(id);

            if (ingredientCategory == null) return NotFound();

            return Ok(ingredientCategory);
        }

        // POST api/ingredientscategories
        [HttpPost]
        public IActionResult Post(IngredientCategoryDTO ingredientCategory)
        {
            if (ingredientCategory == null) return BadRequest();

            var finalIngredientCategory = new IngredientCategory()
            {
                Name = ingredientCategory.Name
            };

            IngredientCategoryService.Add(finalIngredientCategory);

            return CreatedAtRoute("GetIngredientCategory", new { id = finalIngredientCategory.Id }, finalIngredientCategory);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ingredientCategory = IngredientCategoryService.GetIngredientCategory(id);

            if (ingredientCategory == null)
                return NotFound();

            IngredientCategoryService.Remove(ingredientCategory);

            return NoContent();
        }
    }
}