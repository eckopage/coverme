using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace musicapp.Models
{
    public class Instruments
    {
        [Key]
        public int id { get; set; }

        public Nullable<int> userID { get; set; }

        [Required]
        public string instrumentName { get; set; }
        public string instrumentDescription { get; set; }
        public string instrumentPlayingTimeExperience { get; set; }

        public virtual Users Users { get; set; }
    }
}