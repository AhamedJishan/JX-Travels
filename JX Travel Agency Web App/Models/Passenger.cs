using System.ComponentModel.DataAnnotations;

namespace JX_Travel_Agency_Web_App.Models
{
	public class Passenger
	{
		[Key]
		public int PassengerId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
		public int Age { get; set; }
		[Required]
		public string Gender { get; set; }
		[MaxLength(10, ErrorMessage ="Phone number needs 10 digits")]
		[MinLength(10, ErrorMessage = "Phone number needs 10 digits")]
		public string PhoneNumber { get; set;}

		// Relationship
		public Ticket? Ticket { get; set; }
	}
}
