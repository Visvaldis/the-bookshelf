namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnnotation1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tags", "Name", c => c.String());
        }
    }
}
