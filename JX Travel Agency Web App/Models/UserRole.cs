using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JX_Travel_Agency_Web_App.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Role { get; set; } = "User";

        // Relationships
        [ForeignKey(nameof(Id))]
        public virtual User User { get; set; }
    }
}
