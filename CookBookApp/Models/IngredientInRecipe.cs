using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    [Table("IngredientsInRecipes")]
    public class IngredientInRecipe
    {
        public int Id { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public string Quantity { get; set; }
    }
}
