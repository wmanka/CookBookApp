using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models.ViewModels
{
    public class RecipeDetailsViewModel
    {
        public Recipe Recipe { get; set; }

        public IEnumerable<IngredientWithQuantity> Ingredients { get; set; }
    }
}
