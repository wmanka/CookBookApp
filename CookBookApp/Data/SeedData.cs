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

            if (!context.Categories.Any())
            {
                var categories = new List<MealCategory>()
                {
                    new MealCategory() { Name = "Breakfast" },
                    new MealCategory() { Name = "Dessert" },
                    new MealCategory() { Name = "Dinner" },
                    new MealCategory() { Name = "Salad" },
                    new MealCategory() { Name = "Drinks" },
                    new MealCategory() { Name = "Lunch" },
                    new MealCategory() { Name = "Main Courses" },
                    new MealCategory() { Name = "Soup" },
                    new MealCategory() { Name = "Snacks" },
                    new MealCategory() { Name = "Side Dishes" }
                };

                foreach (var category in categories)
                    context.Categories.Add(category);
            }


            if (!context.IngredientCategory.Any())
            {
                var categories = new List<IngredientCategory>()
                {
                    new IngredientCategory() { Name = "Other" },
                    new IngredientCategory() { Name = "Nuts And Oilseeds" },
                    new IngredientCategory() { Name = "Sugar" },
                    new IngredientCategory() { Name = "Seafood" },
                    new IngredientCategory() { Name = "Fruits" },
                    new IngredientCategory() { Name = "Dairy Products" },
                    new IngredientCategory() { Name = "Meat" },
                    new IngredientCategory() { Name = "Careals And Pulses" },
                    new IngredientCategory() { Name = "Spices And Herbs" },
                    new IngredientCategory() { Name = "Vegetables" },
                };

                foreach (var category in categories)
                    context.IngredientCategory.Add(category);
            }


            if (!context.Ingredients.Any())
            {
                var categoryMeat = context.IngredientCategory.Local.Where(a => a.Name == "Meat").First();
                var categoryVegetables = context.IngredientCategory.Local.Where(a => a.Name == "Vegetables").First();
                var categorySpicesAndHerbs = context.IngredientCategory.Local.Where(a => a.Name == "Spices And Herbs").First();
                var categoryCarealsAndPulses = context.IngredientCategory.Local.Where(a => a.Name == "Careals And Pulses").First();
                var categoryDairyProducts = context.IngredientCategory.Local.Where(a => a.Name == "Dairy Products").First();
                var categoryFruits = context.IngredientCategory.Local.Where(a => a.Name == "Fruits").First();
                var categorySeafood = context.IngredientCategory.Local.Where(a => a.Name == "Seafood").First();
                var categorySugar = context.IngredientCategory.Local.Where(a => a.Name == "Sugar").First();
                var categoryNuts = context.IngredientCategory.Local.Where(a => a.Name == "Nuts And Oilseeds").First();
                var categoryOther = context.IngredientCategory.Local.Where(a => a.Name == "Other").First();

                var ingredients = new List<Ingredient>()
                {
                            new Ingredient() { Name = "Tomato", Category = categoryVegetables },
                            new Ingredient() { Name = "Turnip", Category = categoryVegetables },
                            new Ingredient() { Name = "Spinach", Category = categoryVegetables },
                            new Ingredient() { Name = "Onion", Category = categoryVegetables },
                            new Ingredient() { Name = "Mushroom", Category = categoryVegetables },
                            new Ingredient() { Name = "Garlic", Category = categoryVegetables },
                            new Ingredient() { Name = "Cucumber", Category = categoryVegetables },
                            new Ingredient() { Name = "Carrot", Category = categoryVegetables },
                            new Ingredient() { Name = "Broccoli", Category = categoryVegetables },
                            new Ingredient() { Name = "Rosemary", Category = categorySpicesAndHerbs },
                            new Ingredient() { Name = "Salt", Category = categorySpicesAndHerbs },
                            new Ingredient() { Name = "Pepper", Category = categorySpicesAndHerbs },
                            new Ingredient() { Name = "Mint Leaves", Category = categorySpicesAndHerbs },
                            new Ingredient() { Name = "Saffron", Category = categorySpicesAndHerbs },
                            new Ingredient() { Name = "Flour", Category = categoryCarealsAndPulses },
                            new Ingredient() { Name = "Oats", Category = categoryCarealsAndPulses },
                            new Ingredient() { Name = "Beef", Category = categoryMeat },
                            new Ingredient() { Name = "Chicken", Category = categoryMeat },
                            new Ingredient() { Name = "Turkey", Category = categoryMeat },
                            new Ingredient() { Name = "Ham", Category = categoryMeat },
                            new Ingredient() { Name = "Lamb", Category = categoryMeat },
                            new Ingredient() { Name = "Milk", Category = categoryDairyProducts },
                            new Ingredient() { Name = "Ricotta Cheese", Category = categoryDairyProducts },
                            new Ingredient() { Name = "Yogurt", Category = categoryDairyProducts },
                            new Ingredient() { Name = "Mango", Category = categoryFruits },
                            new Ingredient() { Name = "Strawberry", Category = categoryFruits },
                            new Ingredient() { Name = "Papaya", Category = categoryFruits },
                            new Ingredient() { Name = "Shark", Category = categorySeafood },
                            new Ingredient() { Name = "Tuna", Category = categorySeafood },
                            new Ingredient() { Name = "Brown Sugar", Category = categorySugar },
                            new Ingredient() { Name = "Caramel", Category = categorySugar },
                            new Ingredient() { Name = "Pine Nuts", Category = categoryNuts },
                            new Ingredient() { Name = "Cashew Nuts", Category = categoryNuts },
                            new Ingredient() { Name = "Red Wine", Category = categoryOther },
                            new Ingredient() { Name = "Pasta", Category = categoryOther },
                            new Ingredient() { Name = "Jelly", Category = categoryOther },
                };

                foreach (var ingredient in ingredients)
                    context.Ingredients.Add(ingredient);
            }

            await context.SaveChangesAsync();
        }
    }
}
