using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Raport
    {
        public int Id { get; set; }
        public int PassengersNumber { get; set; }
        public decimal FuelCost { get; set; }
        public decimal Distance { get; set; }
        public string DriverId { get; set; }
        public string RouteName { get; set; }
        public DateTime Date { get; set; }
    }
}
