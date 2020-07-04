namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteddatetimeinbook : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "PublishDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "PublishDate", c => c.DateTime());
        }
    }
}
