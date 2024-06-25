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

        [HttpGet("MyWorkouts")]
        public async Task<IActionResult> GetMyWorkouts(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var userWorkouts = await _context.UserWorkouts
                                                .Include(uw => uw.Workout)
                                                .Where(uw => uw.UserId == userId)
                                                .ToListAsync();

                var workouts = userWorkouts.Select(uw => uw.Workout).ToList();

                return Ok(workouts);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveWorkout(UserWorkoutDto userWorkoutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create a new UserWorkout instance with only userId and workoutId
                var userWorkout = new UserWorkout
                {
                    UserId = userWorkoutDto.UserId,
                    WorkoutId = userWorkoutDto.WorkoutId
                };

                _context.UserWorkouts.Add(userWorkout);
                await _context.SaveChangesAsync();
                return Ok("Workout saved successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to save workout: {ex.Message}");
            }
        }



    }
}
