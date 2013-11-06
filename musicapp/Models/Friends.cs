using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace musicapp.Models
{
    public class Friends
    {
        [Key]
        public int id { get; set; }

        [Required]
        public Nullable<int> MainUserID { get; set; }

        [Required]
        public Nullable<int> FriendID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }

        [Required]
        public Nullable<bool> RealFriend { get; set; }
    }
}