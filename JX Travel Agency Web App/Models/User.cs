using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JX_Travel_Agency_Web_App.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // Relationships
        public virtual UserRole Role { get; set; }

    }
}
