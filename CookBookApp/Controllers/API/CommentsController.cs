using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.DTO;
using CookBookApp.Models;
using CookBookApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApp.Controllers.API
{
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext Context;
        private readonly IRecipeService RecipeService;

        public CommentsController(ApplicationDbContext context, IRecipeService recipeService)
        {
            Context = context;
            RecipeService = recipeService;
        }

        [HttpPost]
        public IActionResult Post(CommentDTO vm)
        {
            var comment = new Comment()
            {
                RecipeId = vm.RecipeId,
                UserId = vm.UserId,
                Content = vm.Content,
                CreatedAt = DateTime.Now
            };

            Context.Comments.Add(comment);
            Context.SaveChanges();

            return Ok(comment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComments(int id)
        {
            var recipe = await RecipeService.GetRecipeAsync(id);
            var comments = recipe.Comments.OrderByDescending(c => c.CreatedAt).TakeLast(10);

            return Json(comments);
        }
    }
}