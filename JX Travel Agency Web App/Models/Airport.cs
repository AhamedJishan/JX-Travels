using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JX_Travel_Agency_Web_App.Models
{
    public class Airport
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        // Relationships
        [InverseProperty("DepartureAirport")]
        public virtual ICollection<Flight>? FlightsDeparting { get; set; }

        [InverseProperty("ArrivalAirport")]
        public virtual ICollection<Flight>? FlightsArriving { get; set; }
    }
}
