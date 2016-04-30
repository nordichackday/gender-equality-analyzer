namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blacklist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlacklistedImageUrls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ImageUrl);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BlacklistedImageUrls", new[] { "ImageUrl" });
            DropTable("dbo.BlacklistedImageUrls");
        }
    }
}
