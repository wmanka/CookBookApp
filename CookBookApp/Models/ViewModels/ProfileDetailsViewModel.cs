using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models.ViewModels
{
    public class ProfileDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<MealCategory> MealCategories { get; set; }
    }
}
