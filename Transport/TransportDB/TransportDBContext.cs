using DomainModel;
using System.Data.Entity;

namespace TransportDB
{
    public class TransportDBContext : DbContext
    {
        public TransportDBContext() : base("PerolStationDB")
        {

        }

        public DbSet<Buses> Buses { get; set; }
        public DbSet<BusRoute> Routes { get; set; }
        public DbSet<BusStop> Stops { get; set; }
    }
}
