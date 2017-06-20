using System.Collections.Generic;

namespace DomainModel
{
    public class BusRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BusStop> Stops { get; set; }

        public decimal Price { get; set; }
    }
}