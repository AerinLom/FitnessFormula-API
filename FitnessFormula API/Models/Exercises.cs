using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessFormula_API.Models
{
    public class Exercises
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key for the Exercises table

        public string? Name { get; set; } // Name of the exercise

        public int? Sets { get; set; } // Number of sets for the exercise

        public int? Reps { get; set; } // Number of reps per set for the exercise

        public int? WorkoutId { get; set; } // Foreign key to associate with Workout

        public Workout? Workout { get; set; } // Navigation property to refer to the Workout associated with this exercise
    }
}

