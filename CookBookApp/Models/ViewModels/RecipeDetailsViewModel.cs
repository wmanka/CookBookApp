using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models.ViewModels
{
    public class RecipeDetailsViewModel
    {
        public Recipe Recipe { get; set; }

        //public int RecipePictureId { get; set; }
        //public RecipePicture RecipePicture { get; set; }

        public IEnumerable<IngredientWithQuantity> Ingredients { get; set; }

        public bool IsFavouritedByCurrentUser { get; set; }

        public int NumberOfLikes { get; set; }
    }
}
