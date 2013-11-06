namespace musicapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Name", c => c.String(maxLength: 100));
            AddColumn("dbo.Users", "Surname", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Surname");
            DropColumn("dbo.Users", "Name");
        }
    }
}
