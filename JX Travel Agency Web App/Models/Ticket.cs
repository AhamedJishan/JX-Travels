using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JX_Travel_Agency_Web_App.Models
{
	public class Ticket
	{
		[Key]
		public int TicketId { get; set; }
		[Required]
		public int BookingId { get; set; }
		[Required]
		public int PassengerId { get; set; }
		[Required]
		public int FlightId {  get; set; }
		[Required]
		public string Class { get; set; }
		[Required]
		public int Price {  get; set; }

		// Relationship
		[ForeignKey(nameof(PassengerId))]
		public virtual Passenger Passenger { get; set; }
		[ForeignKey(nameof(BookingId))]
		public virtual Booking Booking { get; set; }
		[ForeignKey(nameof(FlightId))]
		public virtual Flight Flight { get; set; }
	}
}
