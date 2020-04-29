namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRightConnection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "User_Id1", "dbo.Users");
			DropForeignKey("dbo.Books", "AddedByUser_Id", "dbo.Users");
			DropForeignKey("dbo.Books", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.Books", "User_Id", "dbo.Users");
            DropIndex("dbo.Books", new[] { "User_Id" });
            DropIndex("dbo.Books", new[] { "User_Id1" });
            DropIndex("dbo.Books", new[] { "Creator_Id" });
            DropColumn("dbo.Books", "Creator_Id");
            RenameColumn(table: "dbo.Books", name: "User_Id", newName: "Creator_Id");
            CreateTable(
                "dbo.LikedBooks",
                c => new
                    {
                        Book_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_Id, t.User_Id })
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: false)
                .Index(t => t.Book_Id)
                .Index(t => t.User_Id);
            
            AlterColumn("dbo.Books", "Creator_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "Creator_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "Creator_Id");
            AddForeignKey("dbo.Books", "Creator_Id", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.Books", "User_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "User_Id1", c => c.Int());
            DropForeignKey("dbo.Books", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.LikedBooks", "User_Id", "dbo.Users");
            DropForeignKey("dbo.LikedBooks", "Book_Id", "dbo.Books");
            DropIndex("dbo.LikedBooks", new[] { "User_Id" });
            DropIndex("dbo.LikedBooks", new[] { "Book_Id" });
            DropIndex("dbo.Books", new[] { "Creator_Id" });
            AlterColumn("dbo.Books", "Creator_Id", c => c.Int());
            AlterColumn("dbo.Books", "Creator_Id", c => c.Int());
            DropTable("dbo.LikedBooks");
            RenameColumn(table: "dbo.Books", name: "Creator_Id", newName: "User_Id");
            AddColumn("dbo.Books", "Creator_Id", c => c.Int());
            CreateIndex("dbo.Books", "Creator_Id");
            CreateIndex("dbo.Books", "User_Id1");
            CreateIndex("dbo.Books", "User_Id");
            AddForeignKey("dbo.Books", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Books", "Creator_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Books", "User_Id1", "dbo.Users", "Id");
        }
    }
}
