namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deletedaddeddate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "AddedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "AddedDate", c => c.DateTime());
        }
    }
}
