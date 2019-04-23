using CookBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.DTO
{
    public class IngredientDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
        public IngredientCategory Category { get; set; }
    }
}
