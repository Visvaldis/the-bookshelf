namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedfileurltocoverurl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "CoverUrl", c => c.String());
            DropColumn("dbo.Books", "FileUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "FileUrl", c => c.String());
            DropColumn("dbo.Books", "CoverUrl");
        }
    }
}
