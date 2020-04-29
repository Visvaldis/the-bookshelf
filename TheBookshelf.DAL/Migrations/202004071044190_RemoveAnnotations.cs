namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authors", "Name", c => c.String());
            AlterColumn("dbo.Books", "Name", c => c.String());
            AlterColumn("dbo.Tags", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Books", "Name", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Authors", "Name", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
