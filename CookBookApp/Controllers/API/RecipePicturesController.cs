using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace CookBookApp.Controllers.API
{
    [Route("api/recipePictures")]
    public class RecipePicturesController : Controller
    {
        public ApplicationDbContext Context { get; set; }
        public RecipePicturesController(ApplicationDbContext context) => Context = context;

        // GET: api/recipePictures
        [HttpGet]
        public IActionResult Get()
        {
            var recipePictures = Context.RecipePictures.ToList();
            return Ok(recipePictures);
        }

        [HttpPost]
        public IActionResult Post(IFormCollection upload)
        {
            string recipeId = Convert.ToString(upload["recipeId"]);

            var currentpicture = Context.RecipePictures.FirstOrDefault(rp => rp.RecipeId == int.Parse(recipeId));
            try
            {
                var picture = upload.Files[0];

                if (picture != null && picture.Length > 0)
                {
                    var recipePicture = new RecipePicture
                    {
                        ContentType = picture.ContentType,
                        FileName = picture.FileName,
                        RecipeId = int.Parse(recipeId)
                    };

                    using (var reader = new System.IO.BinaryReader(picture.OpenReadStream()))
                    {
                        recipePicture.Content = reader.ReadBytes((int)picture.Length);
                    }

                    if(currentpicture != null)
                    {
                        Context.RecipePictures.Remove(currentpicture);
                    }

                    Context.RecipePictures.Add(recipePicture);
                    Context.SaveChanges();
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Home") });
        }
    }
}