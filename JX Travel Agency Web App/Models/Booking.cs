using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JX_Travel_Agency_Web_App.Models
{
	public class Booking
	{
		[Key]
		public int BookingId { get; set; }
		public DateTime? BookingDate { get; set; }
		[Required]
		public float TotalPrice { get; set; }
		[Required]
		public string Status { get; set; } = "Booked";

		// Relationship
		public virtual ICollection<Ticket>? Tickets { get; set; }
	}
}
