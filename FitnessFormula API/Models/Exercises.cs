using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessFormula_API.Models
{
    public class Exercises
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Sets { get; set; }
        public int? Reps { get; set; }
        public int? WorkoutId { get; set; }
        public Workout? Workout { get; set; }
    }
}
