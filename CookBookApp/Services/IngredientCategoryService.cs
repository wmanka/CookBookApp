using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CookBookApp.Services
{
    public class IngredientCategoryService : IIngredientCategoryService
    {
        private readonly ApplicationDbContext Context;

        public IngredientCategoryService(ApplicationDbContext context) => Context = context;

        public void Add(IngredientCategory ingredienCategory)
        {
            Context.IngredientCategory.Add(ingredienCategory);
            Context.SaveChanges();
        }

        public IEnumerable<IngredientCategory> GetIngredientCategories()
        {
            var ingredientCategories = Context.IngredientCategory.OrderByDescending(i => i.Name).ToList();
            return ingredientCategories;
        }

        public IEnumerable<IngredientCategory> GetIngredientCategories(Expression<Func<IngredientCategory, bool>> predicate)
        {
            var ingredientCategories = Context.IngredientCategory.OrderByDescending(i => i.Name)
                .Where(predicate).ToList();

            return ingredientCategories;
        }

        public IngredientCategory GetIngredientCategory(int id)
        {
            var ingredientCategory = Context.IngredientCategory.FirstOrDefault(ic => ic.Id == id);

            return ingredientCategory;
        }

        public IngredientCategory GetIngredientCategory(Expression<Func<IngredientCategory, bool>> predicate)
        {
            var ingredientCategory = Context.IngredientCategory.FirstOrDefault(predicate);

            return ingredientCategory;
        }

        public void Remove(IngredientCategory ingredienCategory)
        {
            Context.IngredientCategory.Remove(ingredienCategory);
            Context.SaveChanges();
        }

        public void Update(IngredientCategory ingredienCategory)
        {
            Context.IngredientCategory.Update(ingredienCategory);
            Context.SaveChanges();
        }
    }
}
