using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApp.Controllers
{
    [Route("api/ingredientscategories")]
    [ApiController]
    public class IngredientsCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public IngredientsCategoriesController(ApplicationDbContext context) => this.context = context;

        [HttpGet]
        public IActionResult Get()
        {
            var ingredientsCategories = context.IngredientCategory.OrderByDescending(i => i.Name).ToList();

            return Ok(ingredientsCategories);
        }

        // GET api/ingredientscategories/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = context.IngredientCategory.FirstOrDefault(ic => ic.Id == id);

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}