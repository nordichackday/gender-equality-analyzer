namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LastFetched = c.DateTime(nullable: false),
                        Url = c.String(),
                        XpathExpression = c.String(),
                        ParsingStrategy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Articles", "Site_Id", c => c.Int());
            CreateIndex("dbo.Articles", "Site_Id");
            AddForeignKey("dbo.Articles", "Site_Id", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "Site_Id", "dbo.Sites");
            DropIndex("dbo.Articles", new[] { "Site_Id" });
            DropColumn("dbo.Articles", "Site_Id");
            DropTable("dbo.Sites");
        }
    }
}
