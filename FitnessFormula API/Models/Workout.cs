using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessFormula_API.Models
{
    public class Workout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkoutId { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(20)]
        public string? Difficulty { get; set; }

        public int? Duration { get; set; }
        public ICollection<Exercises>? Exercises { get; set; } = new List<Exercises>();
    }
}
