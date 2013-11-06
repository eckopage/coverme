namespace musicapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoCorrelations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OriginalVideoID = c.Int(nullable: false),
                        CoverVideoID = c.Int(nullable: false),
                        DateAssociation = c.DateTime(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.VideoCorrelations", new[] { "UserID" });
            DropForeignKey("dbo.VideoCorrelations", "UserID", "dbo.Users");
            DropTable("dbo.VideoCorrelations");
        }
    }
}
