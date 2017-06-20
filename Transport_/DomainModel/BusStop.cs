using System.ComponentModel.DataAnnotations;

namespace DomainModel
{
    public class BusStop
    {
        public int Id { get; set; }
        public string FirstStop { get; set; }
        public string LastStop { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

    }
}
