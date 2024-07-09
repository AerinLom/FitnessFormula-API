using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessFormula_API.Models
{
    // Represents the relationship between a user and a workout
    public class UserWorkout
    {
        [Key, Column(Order = 0)] // Specifies UserId as part of the composite primary key, order 0
        public int UserId { get; set; }

        [Key, Column(Order = 1)] // Specifies WorkoutId as part of the composite primary key, order 1
        public int WorkoutId { get; set; }

        [ForeignKey("UserId")] // Specifies the foreign key relationship with UserId
        public UserProfile User { get; set; } // Navigation property to access the associated UserProfile

        [ForeignKey("WorkoutId")] // Specifies the foreign key relationship with WorkoutId
        public Workout Workout { get; set; } // Navigation property to access the associated Workout
    }
}
