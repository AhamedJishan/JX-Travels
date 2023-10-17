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
		public int Age { get; set; }
		[Required]
		public string Gender { get; set; }
		[MaxLength(10,ErrorMessage ="Phone Number can't be more than 10 digits!")]
		public int PhoneNumber { get; set;}

		// Relationship
		public Ticket? Ticket { get; set; }
	}
}
