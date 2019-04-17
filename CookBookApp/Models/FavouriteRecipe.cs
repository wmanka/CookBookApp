using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    [Table("FavouriteRecipes")]
    public class FavouriteRecipe
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
