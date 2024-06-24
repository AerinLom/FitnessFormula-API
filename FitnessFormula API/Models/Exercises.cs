using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessFormula_API.Models
{
    public class Exercises
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? Sets { get; set; }
        public int? Reps { get; set; }
        public int? WorkoutId { get; set; }
        [ForeignKey("WorkoutId")]
        public Workout? Workout { get; set; }
    }
}
