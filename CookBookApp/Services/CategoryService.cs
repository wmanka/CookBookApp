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
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(MealCategory category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public IQueryable<MealCategory> GetCategories(Expression<Func<MealCategory, bool>> predicate)
        {
            if (predicate != null)
                return context.Categories.Where(predicate);
            else return context.Categories;
        }

        public IQueryable<MealCategory> GetCategories()
        {
            return context.Categories;
        }

        public MealCategory GetCategory(int id)
        {
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public MealCategory GetCategory(Expression<Func<MealCategory, bool>> predicate)
        {
            return context.Categories.FirstOrDefault(predicate);
        }

        public void Remove(MealCategory category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public void Update(MealCategory category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
        }
    }
}
