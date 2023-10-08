using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JX_Travel_Agency_Web_App.Models
{
    public class SeatInventory
    {
        [Key, ForeignKey("Flight")]
        public int FlightId { get; set; }
        [Key]
        public string Class { get; set; }
        public string SeatCapacity { get; set; }
        public string SeatAvailable { get; set; }
        public float Price { get; set; }

        //Relationships
        public virtual Flight Flight { get; set; }
    }
}
