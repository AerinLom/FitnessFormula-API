using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessFormula_API.Models
{
    public class UserWorkout
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int WorkoutId { get; set; }

        [ForeignKey("UserId")]
        public UserProfile User { get; set; }

        [ForeignKey("WorkoutId")]
        public Workout Workout { get; set; }
    }
}
