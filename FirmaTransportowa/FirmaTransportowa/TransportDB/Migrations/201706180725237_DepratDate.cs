namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepratDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusRoutes", "DepartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusRoutes", "DepartDate");
        }
    }
}
