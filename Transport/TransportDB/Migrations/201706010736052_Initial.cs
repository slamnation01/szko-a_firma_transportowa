namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SeatsNumber = c.Int(nullable: false),
                        AverageFuelUsagePer1km = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusRoutes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Distance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusStops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BusRoute_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusRoutes", t => t.BusRoute_Id)
                .Index(t => t.BusRoute_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BusStops", "BusRoute_Id", "dbo.BusRoutes");
            DropIndex("dbo.BusStops", new[] { "BusRoute_Id" });
            DropTable("dbo.BusStops");
            DropTable("dbo.BusRoutes");
            DropTable("dbo.Buses");
        }
    }
}
