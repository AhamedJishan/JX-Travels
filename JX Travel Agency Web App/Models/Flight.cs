using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JX_Travel_Agency_Web_App.Models
{
    public class Flight
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FlightId { get; set; }
        public string AirlineName { get; set; }
        public string AircraftType { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureAirportCode { get; set; }
        public string ArrivalAirportCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        // Relationships
        public ICollection<SeatInventory>? SeatInventories { get; set; }

        [ForeignKey("DepartureAirportCode")]
        public virtual Airport DepartureAirport { get; set; }

        [ForeignKey("ArrivalAirportCode")]
        public virtual Airport ArrivalAirport { get; set; }
    }
}
