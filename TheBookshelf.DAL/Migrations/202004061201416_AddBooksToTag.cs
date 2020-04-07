namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBooksToTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Book_Id", "dbo.Books");
            DropIndex("dbo.Tags", new[] { "Book_Id" });
            RenameColumn(table: "dbo.Books", name: "AddedByUser_Id", newName: "Creator_Id");
            RenameIndex(table: "dbo.Books", name: "IX_AddedByUser_Id", newName: "IX_Creator_Id");
            CreateTable(
                "dbo.TagBooks",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Book_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Book_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Book_Id);
            
            DropColumn("dbo.Tags", "Book_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Book_Id", c => c.Int());
            DropForeignKey("dbo.TagBooks", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.TagBooks", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagBooks", new[] { "Book_Id" });
            DropIndex("dbo.TagBooks", new[] { "Tag_Id" });
            DropTable("dbo.TagBooks");
            RenameIndex(table: "dbo.Books", name: "IX_Creator_Id", newName: "IX_AddedByUser_Id");
            RenameColumn(table: "dbo.Books", name: "Creator_Id", newName: "AddedByUser_Id");
            CreateIndex("dbo.Tags", "Book_Id");
            AddForeignKey("dbo.Tags", "Book_Id", "dbo.Books", "Id");
        }
    }
}
