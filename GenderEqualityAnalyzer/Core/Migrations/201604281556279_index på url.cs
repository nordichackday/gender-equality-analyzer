namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class indexpåurl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "Url", c => c.String(maxLength: 200));
            CreateIndex("dbo.Articles", "Url");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Articles", new[] { "Url" });
            AlterColumn("dbo.Articles", "Url", c => c.String());
        }
    }
}
