using System;
using System.Collections.Generic;
using System.Text;
using CookBookApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CookBookApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<MealCategory> Categories { get; set; }
        public DbSet<IngredientCategory> IngredientCategory { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientInRecipe> IngredientsInRecipes { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }
        public DbSet<RecipePicture> RecipePictures { get; set; }
        public DbSet<FavouriteRecipe> FavouriteRecipes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FavouriteRecipe>().HasKey(fr => new { fr.UserId, fr.RecipeId });

            builder.Entity<FavouriteRecipe>()
                .HasOne<ApplicationUser>(fr => fr.User)
                .WithMany(u => u.FavouriteRecipes)
                .HasForeignKey(fr => fr.UserId);


            builder.Entity<FavouriteRecipe>()
                .HasOne<Recipe>(fr => fr.Recipe)
                .WithMany(r => r.FavouriteRecipes)
                .HasForeignKey(fr => fr.RecipeId);

            base.OnModelCreating(builder);
        }
    }
}
