namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartDatesList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DDate = c.DateTime(nullable: false),
                        BusRoute_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusRoutes", t => t.BusRoute_Id)
                .Index(t => t.BusRoute_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepartDates", "BusRoute_Id", "dbo.BusRoutes");
            DropIndex("dbo.DepartDates", new[] { "BusRoute_Id" });
            DropTable("dbo.DepartDates");
        }
    }
}
