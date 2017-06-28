namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Raports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Raports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PassengersNumber = c.Int(nullable: false),
                        FuelCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Distance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Raports");
        }
    }
}
