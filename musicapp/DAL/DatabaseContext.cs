using musicapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace musicapp.DAL
{
    public class DatabaseContext: DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<UserRoles> UsersRoles { get; set; }
        public DbSet<VideoTable> VideoTables { get; set; }
        public DbSet<VideoCorrelation> VideoCorrelation { get; set; }
        public DbSet<Friends> friendsConnectedTable { get; set; }
        public DbSet<Instruments> instrumentTable { get; set; }
        public DbSet<MusicTable> musicMusicTable { get; set; }
        public DbSet<TabulatureTable> tabulatureTable { get; set; }
    }
}