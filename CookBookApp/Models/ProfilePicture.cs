using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    [Table("ProfilePictures")]
    public class ProfilePicture : File
    {
        public FileType FileType { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public enum FileType
    {
        Avatar = 1,
        Photo = 2
    }
}
