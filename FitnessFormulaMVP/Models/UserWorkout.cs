namespace FitnessFormulaMVP.Models
{
    public class UserWorkout
    {
        public int UserId { get; set; }
        public int WorkoutId { get; set; }

        public UserModel User { get; set; }
        public WorkoutModel Workout { get; set; }
    }
}
