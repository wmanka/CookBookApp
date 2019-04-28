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
        private readonly ICategoryService CategoryService;

        public CategoriesController(ICategoryService categoryService) => CategoryService = categoryService;

        // GET: api/categories
        [HttpGet]
        public IActionResult Get()
        {
            var result = CategoryService.GetCategories().ToList();

            return Ok(result);
        }

        // GET api/categories/{id}
        [HttpGet("{id}", Name = "GetCategory")]
        public IActionResult Get(int id)
        {
            var result = CategoryService.GetCategory(id);

            if (result == null) return NotFound();

            return Ok(result);
        }

        // POST api/categories
        [HttpPost]
        public IActionResult Post(MealCategoryDTO category)
        {
            if (category == null) return BadRequest();

            if(!ModelState.IsValid)
            {
                return View();
            }

            var finalCategory = new MealCategory()
            {
                Name = category.Name
            };

            CategoryService.Add(finalCategory);

            return CreatedAtRoute("GetCategory", new { id = finalCategory.Id }, finalCategory);
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]MealCategoryDTO category)
        {
            if (category == null) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = CategoryService.GetCategory(id);

            if (result == null)
                return NotFound();

            result.Name = category.Name;
            CategoryService.Update(result);

            return NoContent();
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = CategoryService.GetCategory(id);

            if (category == null)
                return NotFound();

            CategoryService.Remove(category);

            return NoContent();
        }
    }
}
