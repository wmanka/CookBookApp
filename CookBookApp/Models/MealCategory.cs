using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    [Table("MealCategories")]
    public class MealCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
