using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    [Table("RecipePictures")]
    public class RecipePicture : File
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
