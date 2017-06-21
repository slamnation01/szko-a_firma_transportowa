using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModel
{
    public class BusRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BusStop> Stops { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public List<DepartDate> DepartDates { get; set; }

        public string DepartHoursSimple { get; set; }
    }
}
