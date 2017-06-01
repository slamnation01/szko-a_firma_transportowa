namespace Transport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserInfo_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserInfo_Id)
                .Index(t => t.UserInfo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfoes", "UserInfo_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserInfoes", new[] { "UserInfo_Id" });
            DropTable("dbo.UserInfoes");
        }
    }
}
