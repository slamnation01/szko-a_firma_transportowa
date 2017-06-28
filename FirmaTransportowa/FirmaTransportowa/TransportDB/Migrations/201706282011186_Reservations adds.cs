namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reservationsadds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "SeatsNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "SeatsNumber");
        }
    }
}
