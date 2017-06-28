namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driverid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Raports", "DriverId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Raports", "DriverId");
        }
    }
}
