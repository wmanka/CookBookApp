using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models.ViewModels
{
    public class RecipesIndexViewModel
    {
        public IEnumerable<Recipe> Recipes { get; set; }
        public IEnumerable<MealCategory> Categories { get; set; }
        public IEnumerable<RecipePicture> RecipePictures { get; set; }
    }
}
