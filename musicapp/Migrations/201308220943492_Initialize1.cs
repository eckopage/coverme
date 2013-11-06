namespace musicapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicTables", "isOriginal", c => c.Boolean(nullable: false));
            AddColumn("dbo.MusicTables", "isCover", c => c.Boolean(nullable: false));
            AddColumn("dbo.VideoTables", "isOriginal", c => c.Boolean(nullable: false));
            AddColumn("dbo.VideoTables", "isCover", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoTables", "isCover");
            DropColumn("dbo.VideoTables", "isOriginal");
            DropColumn("dbo.MusicTables", "isCover");
            DropColumn("dbo.MusicTables", "isOriginal");
        }
    }
}
