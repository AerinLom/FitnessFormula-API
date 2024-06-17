using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessFormula_API.Models
{
    public class UserProfileInputModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string? Username { get; set; }

        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }
    }
}