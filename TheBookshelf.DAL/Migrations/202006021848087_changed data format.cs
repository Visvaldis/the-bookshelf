﻿namespace TheBookshelf.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeddataformat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "RegistrationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "RegistrationDate", c => c.DateTime(nullable: false));
        }
    }
}
