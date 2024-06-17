using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FitnessFormula_API.Models
{
    [Table("userProfile")]
    public class UserProfile
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

        public string? Preferences { get; set; } // JSON stored as string

        public ICollection<UserWorkout>? UserWorkouts { get; set; }
        //public ICollection<Session> Sessions { get; set; }
    }
}
