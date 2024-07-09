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

        // GET: api/UserWorkouts/MyWorkouts?userId={userId}
        [HttpGet("MyWorkouts")]
        public async Task<IActionResult> GetMyWorkouts(int userId)
        {
            // Validate userId
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                // Retrieve user workouts including related workout details using EF Core Include
                var userWorkouts = await _context.UserWorkouts
                                                .Include(uw => uw.Workout)
                                                .Where(uw => uw.UserId == userId)
                                                .ToListAsync();

                // Extract workouts from UserWorkouts and return as a list
                var workouts = userWorkouts.Select(uw => uw.Workout).ToList();

                return Ok(workouts); // Return list of workouts associated with the user
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                // Example: _logger.LogError(ex, "Error occurred while fetching user workouts");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/UserWorkouts
        [HttpPost]
        public async Task<IActionResult> SaveWorkout(UserWorkoutDto userWorkoutDto)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create a new UserWorkout instance with userId and workoutId from DTO
                var userWorkout = new UserWorkout
                {
                    UserId = userWorkoutDto.UserId,
                    WorkoutId = userWorkoutDto.WorkoutId
                };

                // Add and save the new UserWorkout entity to the database
                _context.UserWorkouts.Add(userWorkout);
                await _context.SaveChangesAsync();

                return Ok("Workout saved successfully!"); // Return success message
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                // Example: _logger.LogError(ex, $"Failed to save workout: {ex.Message}");
                return StatusCode(500, $"Failed to save workout: {ex.Message}");
            }
        }
    }
}
