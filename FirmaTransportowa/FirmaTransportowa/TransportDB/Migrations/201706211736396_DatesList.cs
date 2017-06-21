namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatesList : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BusRoutes", "DepartDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BusRoutes", "DepartDate", c => c.DateTime(nullable: false));
        }
    }
}
