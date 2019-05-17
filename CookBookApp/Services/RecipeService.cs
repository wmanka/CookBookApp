using CookBookApp.Models;
using CookBookApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using CookBookApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CookBookApp.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext Context;
        public RecipeService(ApplicationDbContext context) => Context = context;

        public void Add(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public void AddIngredientsToRecipe(IEnumerable<IngredientInRecipe> ingredients, int recipeId)
        {
            foreach(var ingredient in ingredients)
            {
                var newIngredient = new IngredientInRecipe()
                {
                    IngredientId = ingredient.IngredientId,
                    Quantity = ingredient.Quantity,
                    RecipeId = recipeId
                };

                Context.IngredientsInRecipes.Add(newIngredient);
            }

            Context.SaveChanges();
        }

        public bool CheckIfRecipeIsFavouritedByUser(Recipe recipe, string userId)
        {
            var favouritedRecipes = Context.FavouriteRecipes
               .FirstOrDefault(fr => fr.RecipeId == recipe.Id && fr.UserId == userId);

            if (favouritedRecipes != null) return true;
            else return false;
        }

        public void CreateOrUpdateRecipe(Recipe recipe)
        {
            if (recipe.Id == 0)
            {
                Context.Recipes.Add(recipe);
            }
            else
            {
                Context.Recipes.Update(recipe);

                var currentIngredients = Context.IngredientsInRecipes.Where(i => i.RecipeId == recipe.Id);

                foreach (var ingredient in currentIngredients)
                {
                    Context.IngredientsInRecipes.Remove(ingredient);
                }
            }

            Context.SaveChanges();
        }

        public int GetNumberOfLikes(int id)
        {
            return Context.FavouriteRecipes.Where(fr => fr.RecipeId == id).Count();
        }

        public byte[] GetPdf(int recipeId)
        {
            var recipe = Context.Recipes
                .Include(r => r.Picture)
                .Include(r => r.Category)
                .Include(r => r.User)
                .Include(r => r.Ingredients).ThenInclude(i => i.Ingredient)
                .FirstOrDefault(r => r.Id == recipeId);

            MemoryStream ms = new MemoryStream();

            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font titleFont = new Font(baseFont, 26, Font.BOLD);
            Font labelFont = new Font(baseFont, 12, Font.BOLD);
            Font textFont = new Font(baseFont, 12, Font.NORMAL);
            Font italicFont = new Font(baseFont, 10, Font.ITALIC);

            Document document = new Document(PageSize.A4, 50f, 50f, 25f, 25f);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            document.Open();

            
            if (recipe.Picture != null)
            {
                var picture = new Paragraph();
                Image image = Image.GetInstance(recipe.Picture.Content);
                var scalePercent = (((document.PageSize.Width / image.Width) * 100) - 4);
                image.ScalePercent(scalePercent);
                picture.Add(image);
                document.Add(picture);
            }

            document.Add(new Paragraph(" "));

            var header = new Paragraph
            {
                new Phrase(recipe.Name + "\n", titleFont),
                new Paragraph("By " + recipe.User.Name + " at " + recipe.CreatedAt.ToShortDateString(), italicFont),
                new Paragraph(" "),
                new Phrase(recipe.ShortDescription, textFont)
            };
            document.Add(header);

            document.Add(new Paragraph(" "));

            var info = new Paragraph
            {
                new Chunk("Category: ", labelFont),
                new Phrase(recipe.Category.Name + "\n", textFont),
                new Chunk("Difficulty level: ", labelFont),
                new Phrase(recipe.DifficultyLevel.ToString() + "\n", textFont),
                new Chunk("Preparation time: ", labelFont),
                new Phrase(recipe.PreparationTime, textFont)
            };
            document.Add(info);

            document.Add(new Paragraph(" "));

            var listOfIngredients = new List();
            foreach (var ingredient in recipe.Ingredients.OrderBy(r => r.Ingredient.Name))
            {
                listOfIngredients.Add(new ListItem(" " + ingredient.Ingredient.Name + ": " + ingredient.Quantity, textFont));
            }

            var ingredients = new Paragraph
            {
                new Chunk("Ingredients:" + "\n", labelFont),
                listOfIngredients
            };

            document.Add(ingredients);

            document.Add(new Paragraph(" "));

            document.Add(new Chunk("Instructions:" + "\n", labelFont));
            document.Add(new Paragraph(recipe.Instructions, textFont));

            document.Close();

            return ms.ToArray();
        }

        public string GetPicturePath(RecipePicture picture)
        {
            var path = "data:image/jpeg;base64," +
                    Convert.ToBase64String(picture.Content, 0, picture.Content.Length);

            return path;
        }

        public Recipe GetRecipe(int id)
        {
            var recipe = Context.Recipes
                .Include(r => r.Category).Include(r => r.User)
                .Include(r => r.Comments)
                .Include(r => r.User)
                .Include(r => r.Comments).ThenInclude(r => r.User)
                .Include(r => r.Ingredients).ThenInclude(r => r.Ingredient)
                .FirstOrDefault(r => r.Id == id);

            return recipe;
        }

        public async Task<Recipe> GetRecipeAsync(int id)
        {
            var recipe = Context.Recipes
                .Include(r => r.Category).Include(r => r.User)
                .Include(r => r.User)
                .Include(r => r.Comments).ThenInclude(r => r.User)
                .Include(r => r.Ingredients).ThenInclude(r => r.Ingredient)
                .FirstAsync(r => r.Id == id);

            return await recipe;
        }

        public Recipe GetRecipe(Expression<Func<Recipe, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public RecipePicture GetRecipePicture(int id)
        {
            return Context.RecipePictures.FirstOrDefault(rp => rp.RecipeId == id);
        }

        public IQueryable<Recipe> GetRecipes()
        {
            return Context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Picture)
                .OrderByDescending(r => r.CreatedAt);
        }

        public IQueryable<Recipe> GetRecipes(Expression<Func<Recipe, bool>> predicate)
        {
            return Context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Picture)
                .Where(predicate)
                .OrderByDescending(r => r.CreatedAt);
        }

        public void Remove(int id)
        {
            var recipe = Context.Recipes.FirstOrDefault(r => r.Id == id);

            Context.Recipes.Remove(recipe);
            Context.SaveChanges();
        }

        public void Update(Recipe recipe)
        {
            throw new NotImplementedException();
        }
    }
}
