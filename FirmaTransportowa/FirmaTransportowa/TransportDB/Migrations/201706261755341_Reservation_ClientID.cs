namespace TransportDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reservation_ClientID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "ClientID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "ClientID");
        }
    }
}
