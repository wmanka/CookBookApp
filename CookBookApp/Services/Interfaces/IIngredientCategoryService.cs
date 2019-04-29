using CookBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CookBookApp.Services.Interfaces
{
    public interface IIngredientCategoryService
    {
        IEnumerable<IngredientCategory> GetIngredientCategories();
        IEnumerable<IngredientCategory> GetIngredientCategories(Expression<Func<IngredientCategory, bool>> predicate);
        IngredientCategory GetIngredientCategory(int id);
        IngredientCategory GetIngredientCategory(Expression<Func<IngredientCategory, bool>> predicate);
        void Add(IngredientCategory ingredienCategory);
        void Update(IngredientCategory ingredienCategory);
        void Remove(IngredientCategory ingredienCategory);
    }
}
