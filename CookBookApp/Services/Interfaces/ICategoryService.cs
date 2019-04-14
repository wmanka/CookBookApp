using CookBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CookBookApp.Services.Interfaces
{
    public interface ICategoryService
    {
        IQueryable<MealCategory> GetCategories();
        IQueryable<MealCategory> GetCategories(Expression<Func<MealCategory, bool>> predicate);
        MealCategory GetCategory(int id);
        MealCategory GetCategory(Expression<Func<MealCategory, bool>> predicate);
        void Add(MealCategory category);
        void Update(MealCategory category);
        void Remove(MealCategory category);
    }
}
