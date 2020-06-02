namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editusermodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "RegistrationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "RegistrationDate", c => c.DateTime(nullable: false));
        }
    }
}
