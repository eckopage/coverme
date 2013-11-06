namespace musicapp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using musicapp.Models;
    using musicapp.DAL.Repositories;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<musicapp.DAL.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(musicapp.DAL.DatabaseContext context)
        {

        }
    }
}
