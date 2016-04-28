namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class faces : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                        ImageUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        IsImageParsed = c.Boolean(nullable: false),
                        ContainsPerson = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Faces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FaceId = c.String(),
                        Gender = c.Int(nullable: false),
                        Age = c.Int(),
                        SmileFactor = c.Double(nullable: false),
                        HeadRoll = c.Double(nullable: false),
                        HeadYaw = c.Double(nullable: false),
                        HeadPitch = c.Double(nullable: false),
                        MoustacheFactor = c.Double(nullable: false),
                        BeardFactor = c.Double(nullable: false),
                        SideburnsFactor = c.Double(nullable: false),
                        Article_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.Article_Id)
                .Index(t => t.Article_Id);
            
            DropTable("dbo.Tests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        test = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Faces", "Article_Id", "dbo.Articles");
            DropIndex("dbo.Faces", new[] { "Article_Id" });
            DropTable("dbo.Faces");
            DropTable("dbo.Articles");
        }
    }
}
