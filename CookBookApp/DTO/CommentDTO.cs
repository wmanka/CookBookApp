using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.DTO
{
    public class CommentDTO
    {
        public int RecipeId { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }
    }
}
