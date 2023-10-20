using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JX_Travel_Agency_Web_App.Models
{
	public class Booking
	{
		[Key]
		public int BookingId { get; set; }
		public int UserId { get; set; }
		public DateTime? BookingDate { get; set; }
		[Required]
		public float TotalPrice { get; set; }
		[Required]
		public string Status { get; set; } = "Booked";

		// Relationship
		public virtual ICollection<Ticket>? Tickets { get; set; }
		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; }
	}
}
