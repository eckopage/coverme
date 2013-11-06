using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace musicapp.Models
{
    public class UserRoles
    {
        public UserRoles()
        {
            this.Users = new HashSet<Users>();
        }

        [Key]
        public int RoleId { get; set; }
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
    
        public virtual ICollection<Users> Users { get; set; }
    }
}