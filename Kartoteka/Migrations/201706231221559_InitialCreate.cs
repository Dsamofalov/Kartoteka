namespace Kartoteka.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Genre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthorBook",
                c => new
                    {
                        AuthorID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuthorID, t.BookID })
                .ForeignKey("dbo.AuthorModels", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.BookModels", t => t.BookID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.BookID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorBook", "BookID", "dbo.BookModels");
            DropForeignKey("dbo.AuthorBook", "AuthorID", "dbo.AuthorModels");
            DropIndex("dbo.AuthorBook", new[] { "BookID" });
            DropIndex("dbo.AuthorBook", new[] { "AuthorID" });
            DropTable("dbo.AuthorBook");
            DropTable("dbo.BookModels");
            DropTable("dbo.AuthorModels");
        }
    }
}
