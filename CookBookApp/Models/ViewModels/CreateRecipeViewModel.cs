using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models.ViewModels
{
    public class CreateRecipeViewModel
    {
        public Recipe Recipe { get; set; }

        [Display(Name = "Category")]
        public int MealCategoryId { get; set; }

        public IEnumerable<MealCategory> MealCategories { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public IEnumerable<IngredientInRecipe> ChosenIngredients { get; set; }
    }
}
