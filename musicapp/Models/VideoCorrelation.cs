using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace musicapp.Models
{
    public class VideoCorrelation
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int OriginalVideoID { get; set; }

        [Required]
        public int CoverVideoID { get; set; }

        [Required]
        public System.DateTime DateAssociation { get; set; }

        [Required]
        public int UserID { get; set; }

        public virtual Users Users { get; set; }
    }
}