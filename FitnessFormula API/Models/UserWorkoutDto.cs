namespace FitnessFormula_API.Models
{
    // Data Transfer Object (DTO) for UserWorkout information
    public class UserWorkoutDto
    {
        public int UserId { get; set; } // Gets or sets the UserId associated with the UserWorkout

        public int WorkoutId { get; set; } // Gets or sets the WorkoutId associated with the UserWorkout
    }
}
