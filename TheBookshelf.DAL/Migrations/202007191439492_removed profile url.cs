namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedprofileurl : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "AvatarUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "AvatarUrl", c => c.String());
        }
    }
}
