using System.ComponentModel.DataAnnotations;

namespace JX_Travel_Agency_Web_App.Models
{
    public class Airport
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
