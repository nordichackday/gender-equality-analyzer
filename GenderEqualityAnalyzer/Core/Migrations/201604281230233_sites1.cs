namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sites1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sites", "LastFetched", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sites", "LastFetched", c => c.DateTime(nullable: false));
        }
    }
}
