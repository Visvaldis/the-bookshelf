namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deletedconnectionbookcreator : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "CreatorId", "dbo.Users");
            DropIndex("dbo.Books", new[] { "CreatorId" });
            DropColumn("dbo.Books", "CreatorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "CreatorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "CreatorId");
            AddForeignKey("dbo.Books", "CreatorId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
