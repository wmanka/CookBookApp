using CookBookApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Data
{
    public class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            string adminId = "";

            string roleAdmin = "Admin";
            string roleUser = "User";

            string password = "Password1!";

            if (await roleManager.FindByNameAsync(roleAdmin) == null)
                await roleManager.CreateAsync(new IdentityRole(roleAdmin));

            if (await roleManager.FindByNameAsync(roleUser) == null)
                await roleManager.CreateAsync(new IdentityRole(roleUser));

            if (await userManager.FindByNameAsync("admin@admin.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    PhoneNumber = "123456789"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, roleAdmin);
                }
                adminId = user.Id;
            }

            if (await userManager.FindByNameAsync("user2@email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user2@email.com",
                    Email = "user2@email.com",
                    PhoneNumber = "1029384756",
                    Location = "Warsaw",
                    Name = "Jan Kowalski"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, roleUser);
                }
            }

            if (await userManager.FindByNameAsync("user3@email.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user3@email.com",
                    Email = "user3@email.com",
                    PhoneNumber = "8822993300441",
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, roleUser);
                }
            }

            //if (!context.Categories.Any())
            //{
            //    var categories = new List<MealCategory>()
            //    {
            //        new MealCategory() { Name = "Breakfast" },
            //        new MealCategory() { Name = "Dessert" },
            //        new MealCategory() { Name = "Dinner" },
            //        new MealCategory() { Name = "Salad" },
            //        new MealCategory() { Name = "Drinks" },
            //        new MealCategory() { Name = "Lunch" },
            //        new MealCategory() { Name = "Main Courses" },
            //        new MealCategory() { Name = "Soup" },
            //        new MealCategory() { Name = "Snacks" },
            //        new MealCategory() { Name = "Side Dishes" }
            //    };

            //    foreach (var category in categories)
            //        context.Categories.Add(category);
            //}


            //if (!context.IngredientCategory.Any())
            //{
            //    var categories = new List<IngredientCategory>()
            //    {
            //        new IngredientCategory() { Name = "Other" },
            //        new IngredientCategory() { Name = "Nuts And Oilseeds" },
            //        new IngredientCategory() { Name = "Sugar" },
            //        new IngredientCategory() { Name = "Seafood" },
            //        new IngredientCategory() { Name = "Fruits" },
            //        new IngredientCategory() { Name = "Dairy Products" },
            //        new IngredientCategory() { Name = "Meat" },
            //        new IngredientCategory() { Name = "Careals And Pulses" },
            //        new IngredientCategory() { Name = "Spices And Herbs" },
            //        new IngredientCategory() { Name = "Vegetables" },
            //    };

            //    foreach (var category in categories)
            //        context.IngredientCategory.Add(category);
            //}


            //if (!context.Ingredients.Any())
            //{
            //    var category = context.IngredientCategory.Local.Where(a => a.Name == "Meat").First();
            //    var ingredients = new List<Ingredient>()
            //    {
            //        //        new Ingredient() { Name = "Tomato", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Turnip", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Spinach", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Onion", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Mushroom", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Garlic", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Cucumber", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Carrot", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Broccoli", CategoryId = 1 },
            //        //        new Ingredient() { Name = "Rosemary", CategoryId = 2 },
            //        //        new Ingredient() { Name = "Salt", CategoryId = 2 },
            //        //        new Ingredient() { Name = "Pepper", CategoryId = 2 },
            //        //        new Ingredient() { Name = "Mint Leaves", CategoryId = 2 },
            //        //        new Ingredient() { Name = "Saffron", CategoryId = 2 },
            //        //        new Ingredient() { Name = "Flour", CategoryId = 3 },
            //        //        new Ingredient() { Name = "Oats", CategoryId = 3 },
            //                new Ingredient() { Name = "Beef", Category = category },
            //                new Ingredient() { Name = "Chicken", Category = category },
            //                new Ingredient() { Name = "Turkey", Category = category },
            //                new Ingredient() { Name = "Ham", Category = category },
            //                new Ingredient() { Name = "Lamb", Category = category },
            //        //        new Ingredient() { Name = "Milk", CategoryId = 5 },
            //        //        new Ingredient() { Name = "Ricotta Cheese", CategoryId = 5 },
            //        //        new Ingredient() { Name = "Yogurt", CategoryId = 5 },
            //        //        new Ingredient() { Name = "Mango", CategoryId = 6 },
            //        //        new Ingredient() { Name = "Strawberry", CategoryId = 6 },
            //        //        new Ingredient() { Name = "Papaya", CategoryId = 6 },
            //        //        new Ingredient() { Name = "Shark", CategoryId = 7 },
            //        //        new Ingredient() { Name = "Tuna", CategoryId = 7 },
            //        //        new Ingredient() { Name = "Brown Sugar", CategoryId = 8 },
            //        //        new Ingredient() { Name = "Caramel", CategoryId = 8 },
            //        //        new Ingredient() { Name = "Pine Nuts", CategoryId = 9 },
            //        //        new Ingredient() { Name = "Cashew Nuts", CategoryId = 9 },
            //        //        new Ingredient() { Name = "Red Wine", CategoryId = 10 },
            //        //        new Ingredient() { Name = "Pasta", CategoryId = 10 },
            //        //        new Ingredient() { Name = "Jelly", CategoryId = 10 },
            //    };

            //    foreach (var ingredient in ingredients)
            //        context.Ingredients.Add(ingredient);
            //}

            await context.SaveChangesAsync();
        }
    }
}
