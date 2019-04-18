using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public ICollection<ProfilePicture> Files { get; set; }

        public ICollection<Recipe> Recipes { get; set; }

        public ICollection<FavouriteRecipe> FavouriteRecipes { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
