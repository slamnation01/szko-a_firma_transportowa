namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FInal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusRoutes", "DepartHoursSimple", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusRoutes", "DepartHoursSimple");
        }
    }
}
