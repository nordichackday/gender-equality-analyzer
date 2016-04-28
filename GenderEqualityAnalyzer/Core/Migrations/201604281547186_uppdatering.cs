namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uppdatering : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Faces", "FaceId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Faces", "Age", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Faces", "Age", c => c.Int());
            AlterColumn("dbo.Faces", "FaceId", c => c.String());
        }
    }
}
