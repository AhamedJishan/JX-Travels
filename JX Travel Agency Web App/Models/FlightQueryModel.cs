using JX_Travel_Agency_Web_App.Data.Enums;

namespace JX_Travel_Agency_Web_App.Models
{
    public class FlightQueryModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateOnly Departure { get; set; }
        public int Passengers { get; set; }
        public Class Class { get; set; }
    }
}
