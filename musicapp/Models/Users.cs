using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace musicapp.Models
{
    public class Users
    {
        public Users()
        {
            this.musicMusicTable = new HashSet<MusicTable>();
            this.VideoTables = new HashSet<VideoTable>();
            this.tabulatureTable = new HashSet<TabulatureTable>();
            this.VideoCorrelations = new HashSet<VideoCorrelation>();
            this.instrumentTable = new HashSet<Instruments>();
        }
    
        [Key]
        public int UserId { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string Password { get; set; }

        [Required]
        [StringLength(250)]
        public string PasswordSalt { get; set; }

        [Required]
        public System.DateTime CreatedDate { get; set; }

        public System.DateTime? LastModifiedDate { get; set; }

        public System.DateTime? LastLoginDate { get; set; }

        [Required]
        public bool IsActivated { get; set; }

        [StringLength(250)]
        public string AccountActivationCode { get; set; }

        [StringLength(250)]
        public string PasswordResetCode { get; set; }
    
        public virtual UserRoles UsersRoles { get; set; }
        public virtual ICollection<MusicTable> musicMusicTable { get; set; }
        public virtual ICollection<VideoTable> VideoTables { get; set; }
        public virtual ICollection<TabulatureTable> tabulatureTable { get; set; }
        public virtual ICollection<VideoCorrelation> VideoCorrelations { get; set; }
        public virtual ICollection<Instruments> instrumentTable { get; set; }
    }
}