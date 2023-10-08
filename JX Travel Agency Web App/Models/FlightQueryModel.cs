using JX_Travel_Agency_Web_App.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JX_Travel_Agency_Web_App.Models
{
    public class FlightQueryModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Departure { get; set; }
        public int Passengers { get; set; }

        public Class Class { get; set; }
        public List<SelectListItem> ClassTypes { get; set; }

        public FlightQueryModel()
        {
            ClassTypes = new List<SelectListItem>();
            foreach (Class classType in Enum.GetValues(typeof(Class)))
            {
                ClassTypes.Add(new SelectListItem
                {
                    Value = classType.ToString(),
                    Text = classType.ToString()
                });
            }
        }

    }
}
