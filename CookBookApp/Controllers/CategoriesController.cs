using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.DTO;
using CookBookApp.Models;
using CookBookApp.Services;
using CookBookApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApp.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public IActionResult Get()
        {
            var result = categoryService.GetCategories().ToList();
            return Ok(result);
        }

        // GET api/categories/{id}
        [HttpGet("{id}", Name = "GetCategory")]
        public IActionResult Get(int id)
        {
            var result = categoryService.GetCategory(id);

            if (result == null) return NotFound();
            else return Ok(result);
        }

        // POST api/categories
        [HttpPost]
        public IActionResult Post(MealCategoryDTO category)
        {
            if (category == null) return BadRequest();

            var finalCategory = new MealCategory()
            {
                Name = category.Name
            };

            categoryService.Add(finalCategory);
            return CreatedAtRoute("GetCategory", new { id = finalCategory.Id }, finalCategory); // Set Custom Name Parameter for HttpGet!
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]MealCategoryDTO category)
        {
            if (category == null) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = categoryService.GetCategory(id);

            if (result == null) return NotFound();

            result.Name = category.Name;
            categoryService.Update(result);

            return NoContent();

        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = categoryService.GetCategory(id);

            if (category == null) return NotFound();
            else categoryService.Remove(category);

            return NoContent();

        }
    }
}
