namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Raportv20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Raports", "RouteName", c => c.String());
            AddColumn("dbo.Raports", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Raports", "Date");
            DropColumn("dbo.Raports", "RouteName");
        }
    }
}
