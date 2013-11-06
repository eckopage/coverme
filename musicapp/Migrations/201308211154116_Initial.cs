namespace musicapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 250),
                        PasswordSalt = c.String(nullable: false, maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        IsActivated = c.Boolean(nullable: false),
                        AccountActivationCode = c.String(maxLength: 250),
                        PasswordResetCode = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.UserRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.MusicTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Artist = c.String(nullable: false),
                        TypeExt = c.String(nullable: false),
                        DateUpload = c.DateTime(nullable: false),
                        Guid = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Description = c.String(),
                        Path = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                        Flex = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VideoTables",
                c => new
                    {
                        musicID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        musicName = c.String(nullable: false),
                        musicArtist = c.String(nullable: false),
                        musicTypeExtension = c.String(nullable: false),
                        musicDateUpload = c.DateTime(nullable: false),
                        musicGuid = c.String(nullable: false),
                        musicType = c.String(nullable: false),
                        musicDescription = c.String(),
                        musicPath = c.String(nullable: false),
                        musicFileName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.musicID);
            
            CreateTable(
                "dbo.TabulatureTables",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userID = c.Int(),
                        coverID = c.Int(),
                        intrumentID = c.Int(),
                        tabSitePath = c.String(),
                        tabFolderPath = c.String(),
                        tabName = c.String(),
                        tabExtension = c.String(),
                        tabDescription = c.String(),
                        tabContent = c.String(),
                        tabUploadTime = c.DateTime(),
                        tabGuid = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.userID)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.Instruments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userID = c.Int(),
                        instrumentName = c.String(nullable: false),
                        instrumentDescription = c.String(),
                        instrumentPlayingTimeExperience = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.userID)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        MainUserID = c.Int(nullable: false),
                        FriendID = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        RealFriend = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.MusicTableUsers",
                c => new
                    {
                        MusicTable_ID = c.Int(nullable: false),
                        Users_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MusicTable_ID, t.Users_UserId })
                .ForeignKey("dbo.MusicTables", t => t.MusicTable_ID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Users_UserId, cascadeDelete: true)
                .Index(t => t.MusicTable_ID)
                .Index(t => t.Users_UserId);
            
            CreateTable(
                "dbo.VideoTableUsers",
                c => new
                    {
                        VideoTable_musicID = c.Int(nullable: false),
                        Users_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoTable_musicID, t.Users_UserId })
                .ForeignKey("dbo.VideoTables", t => t.VideoTable_musicID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Users_UserId, cascadeDelete: true)
                .Index(t => t.VideoTable_musicID)
                .Index(t => t.Users_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.VideoTableUsers", new[] { "Users_UserId" });
            DropIndex("dbo.VideoTableUsers", new[] { "VideoTable_musicID" });
            DropIndex("dbo.MusicTableUsers", new[] { "Users_UserId" });
            DropIndex("dbo.MusicTableUsers", new[] { "MusicTable_ID" });
            DropIndex("dbo.Instruments", new[] { "userID" });
            DropIndex("dbo.TabulatureTables", new[] { "userID" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropForeignKey("dbo.VideoTableUsers", "Users_UserId", "dbo.Users");
            DropForeignKey("dbo.VideoTableUsers", "VideoTable_musicID", "dbo.VideoTables");
            DropForeignKey("dbo.MusicTableUsers", "Users_UserId", "dbo.Users");
            DropForeignKey("dbo.MusicTableUsers", "MusicTable_ID", "dbo.MusicTables");
            DropForeignKey("dbo.Instruments", "userID", "dbo.Users");
            DropForeignKey("dbo.TabulatureTables", "userID", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.UserRoles");
            DropTable("dbo.VideoTableUsers");
            DropTable("dbo.MusicTableUsers");
            DropTable("dbo.Friends");
            DropTable("dbo.Instruments");
            DropTable("dbo.TabulatureTables");
            DropTable("dbo.VideoTables");
            DropTable("dbo.MusicTables");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
        }
    }
}
