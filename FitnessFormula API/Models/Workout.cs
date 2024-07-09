using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FitnessFormula_API.Models
{
    // Represents a workout entity
    public class Workout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkoutId { get; set; } // Primary key for the Workout entity

        [MaxLength(100)]
        public string? Name { get; set; } // Name of the workout, limited to 100 characters

        [MaxLength(50)]
        public string? Category { get; set; } // Category of the workout, limited to 50 characters

        [MaxLength(20)]
        public string? Difficulty { get; set; } // Difficulty level of the workout, limited to 20 characters

        public int? Duration { get; set; } // Duration of the workout in minutes

        public ICollection<Exercises>? Exercises { get; set; } // Collection navigation property for related Exercises
    }
}
