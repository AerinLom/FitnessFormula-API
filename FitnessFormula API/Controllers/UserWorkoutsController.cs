using Microsoft.AspNetCore.Mvc;
using FitnessFormula_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitnessFormula_API.Data;

namespace FitnessFormula_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWorkoutsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserWorkoutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserWorkouts/{userId}/Workouts
        [HttpGet("{userId}/Workouts")]
        public async Task<ActionResult<IEnumerable<Workout>>> GetUserWorkouts(int userId)
        {
            var user = await _context.UserProfiles
                .Include(u => u.UserWorkouts)
                .ThenInclude(uw => uw.Workout)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var workouts = user.UserWorkouts.Select(uw => uw.Workout).ToList();
            return Ok(workouts);
        }

        // POST: api/UserWorkouts
        [HttpPost]
        public async Task<IActionResult> PostUserWorkout(int userId, int workoutId)
        {
            try
            {
                var user = await _context.UserProfiles.FindAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var workout = await _context.Workout.FindAsync(workoutId);
                if (workout == null)
                {
                    return NotFound("Workout not found");
                }

                var existingUserWorkout = await _context.UserWorkouts.FindAsync(userId, workoutId);
                if (existingUserWorkout != null)
                {
                    return Conflict("Workout already exists in user's list");
                }

                var userWorkout = new UserWorkout
                {
                    UserId = userId,
                    WorkoutId = workoutId,
                    User = user,
                    Workout = workout
                };

                _context.UserWorkouts.Add(userWorkout);
                await _context.SaveChangesAsync();

                return Ok("Workout added to user's list successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to add workout to user's list: {ex.Message}");
            }
        }
    }
}
