using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CookBookApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext Context;

        public CategoryService(ApplicationDbContext context) => Context = context;

        public void Add(MealCategory category)
        {
            Context.Categories.Add(category);
            Context.SaveChanges();
        }

        public IQueryable<MealCategory> GetCategories(Expression<Func<MealCategory, bool>> predicate)
        {
            if (predicate != null)
                return Context.Categories.Where(predicate);

            return Context.Categories;
        }

        public IQueryable<MealCategory> GetCategories()
        {
            return Context.Categories;
        }

        public MealCategory GetCategory(int id)
        {
            return Context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public MealCategory GetCategory(Expression<Func<MealCategory, bool>> predicate)
        {
            return Context.Categories.FirstOrDefault(predicate);
        }

        public void Remove(MealCategory category)
        {
            Context.Categories.Remove(category);
            Context.SaveChanges();
        }

        public void Update(MealCategory category)
        {
            Context.Categories.Update(category);
            Context.SaveChanges();
        }
    }
}
