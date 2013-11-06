using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace musicapp.Models
{
    public class MusicTable
    {
        public MusicTable()
        {
            this.Users = new HashSet<Users>();
        }
    
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        public string TypeExt { get; set; }

        [Required]
        public System.DateTime DateUpload { get; set; }

        [Required]
        public string Guid { get; set; }

        [Required]
        public string Type { get; set; }

        public string Description { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string FileName { get; set; }

        public string Flex { get; set; }

        [DefaultValue(true)]
        public bool isOriginal { get; set; }

        [DefaultValue(false)]
        public bool isCover { get; set; }
    
        public virtual ICollection<Users> Users { get; set; }
    }
}