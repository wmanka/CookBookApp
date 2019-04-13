using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    [Table("Files")]
    public class File
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

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
