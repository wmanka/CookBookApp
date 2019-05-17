using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
