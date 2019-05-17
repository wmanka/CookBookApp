using CookBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CookBookApp.Services.Interfaces
{
    public interface IRecipeService
    {
        IQueryable<Recipe> GetRecipes();
        IQueryable<Recipe> GetRecipes(Expression<Func<Recipe, bool>> predicate);
        Recipe GetRecipe(int id);
        Task<Recipe> GetRecipeAsync(int id);
        Recipe GetRecipe(Expression<Func<Recipe, bool>> predicate);
        void Add(Recipe recipe);
        void Update(Recipe recipe);
        void Remove(int id);
        RecipePicture GetRecipePicture(int id);

        byte[] GetPdf(int id);
        string GetPicturePath(RecipePicture picture);
        int GetNumberOfLikes(int id);
        bool CheckIfRecipeIsFavouritedByUser(Recipe recipe, string userId);
        void CreateOrUpdateRecipe(Recipe recipe);
        void AddIngredientsToRecipe(IEnumerable<IngredientInRecipe> ingredients, int recipeId);
    }
}
