using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models.ViewModels
{
    public class IngredientWithQuantity
    {
        public Ingredient Ingredient { get; set; }

        public string Quantity { get; set; }
    }
}
