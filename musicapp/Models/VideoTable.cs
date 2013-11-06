using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace musicapp.Models
{
    public class VideoTable
    {
        public VideoTable()
        {
            this.Users = new HashSet<Users>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int musicID {get; set;}

        [Required]
        public int UserId { get; set; }

        [Required]
        public string musicName {get; set;}
        [Required]
        public string musicArtist {get; set;}
        [Required]
        public string musicTypeExtension {get; set;}
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime musicDateUpload {get; set;}
        [Required]
        public string musicGuid {get; set;}
        [Required]
        public string musicType {get; set;}

        public string musicDescription {get; set;}
        [Required]
        public string musicPath {get; set;}
        [Required]
        public string musicFileName { get; set; }

        [DefaultValue(true)]
        public bool isOriginal { get; set; }

        [DefaultValue(false)]
        public bool isCover { get; set; }
    
        public virtual ICollection<Users> Users { get; set; }
    }
}