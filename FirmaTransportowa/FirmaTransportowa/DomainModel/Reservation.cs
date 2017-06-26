using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ClientID { get; set; }
        public string PassengerName { get; set; }
        public string ClientName { get; set; }
        public string RouteName { get; set; }
        public DateTime Date { get; set; }
    }
}
