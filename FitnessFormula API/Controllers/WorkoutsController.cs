using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessFormula_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessFormula_API.Data;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace FitnessFormula_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WorkoutsController> _logger;

        public WorkoutsController(ApplicationDbContext context, ILogger<WorkoutsController> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Workouts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
        {
            try
            {
                // Retrieve all workouts including exercises using EF Core Include
                var workouts = await _context.Workout
                    .Include(w => w.Exercises)
                    .ToListAsync();

                // Configure JsonSerializerOptions with ReferenceHandler.Preserve
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                // Serialize workouts with the configured options
                var jsonWorkouts = JsonSerializer.Serialize(workouts, options);

                return Ok(jsonWorkouts); // Return serialized workouts
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                _logger.LogError(ex, "Error occurred while fetching workouts.");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Workouts/name/{name}
        [HttpGet("name/{name}")]
        public async Task<ActionResult<Workout>> GetWorkoutByName(string name)
        {
            // Validate workout name
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Workout name cannot be empty.");
            }

            try
            {
                // Retrieve workout including exercises by name using EF Core Include and case-insensitive search
                var workout = await _context.Workout
                    .Include(w => w.Exercises)
                    .FirstOrDefaultAsync(w => w.Name.ToLower() == name.ToLower());

                if (workout == null)
                {
                    return NotFound("Workout not found.");
                }

                // Configure JsonSerializerOptions with ReferenceHandler.Preserve to handle cycles
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 32 // Increase depth limit if needed
                };

                // Serialize workout using configured options
                var jsonWorkout = JsonSerializer.Serialize(workout, options);

                return Ok(jsonWorkout); // Return serialized workout
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                _logger.LogError(ex, $"Error occurred while fetching workout by name: {name}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Workouts/category/{category}
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkoutsByCategory(string category)
        {
            try
            {
                // Retrieve workouts including exercises by category using EF Core Include
                var workouts = await _context.Workout
                    .Include(w => w.Exercises)
                    .Where(w => w.Category == category)
                    .ToListAsync();

                if (workouts == null || workouts.Count == 0)
                {
                    return NotFound("No workouts found.");
                }

                // Configure JsonSerializerOptions with ReferenceHandler.Preserve
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                // Serialize workouts with the configured options
                var jsonWorkouts = JsonSerializer.Serialize(workouts, options);

                return Ok(jsonWorkouts); // Return serialized workouts
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                _logger.LogError(ex, $"Error occurred while fetching workouts by category: {category}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Workouts/Search/{workoutName}
        [HttpGet("Search/{workoutName}")]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkoutsByName(string workoutName)
        {
            // Validate workout name
            if (string.IsNullOrWhiteSpace(workoutName))
            {
                return BadRequest("Workout name cannot be empty.");
            }

            try
            {
                // Retrieve workouts including exercises by partial name match using EF Core Include and case-insensitive search
                var workouts = await _context.Workout
                    .Include(w => w.Exercises)
                    .Where(w => w.Name.ToLower().Contains(workoutName.ToLower()))
                    .ToListAsync();

                if (workouts == null || workouts.Count == 0)
                {
                    return NotFound("No workouts found.");
                }

                // Configure JsonSerializerOptions with ReferenceHandler.Preserve
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                // Serialize workouts with the configured options
                var jsonWorkouts = JsonSerializer.Serialize(workouts, options);

                return Ok(jsonWorkouts); // Return serialized workouts
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                _logger.LogError(ex, $"Error occurred while searching workouts by name: {workoutName}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Workouts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, [FromBody] Workout updatedWorkout)
        {
            // Validate ID match
            if (id != updatedWorkout.WorkoutId)
            {
                return BadRequest();
            }

            try
            {
                // Retrieve existing workout including exercises
                var existingWorkout = await _context.Workout
                    .Include(w => w.Exercises)
                    .FirstOrDefaultAsync(w => w.WorkoutId == id);

                if (existingWorkout == null)
                {
                    return NotFound();
                }

                // Add new exercises to the existing workout
                foreach (var exercise in updatedWorkout.Exercises)
                {
                    existingWorkout.Exercises.Add(exercise);
                }

                // Update the workout in the database
                _context.Entry(existingWorkout).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent(); // HTTP 204
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Bubble up the exception for logging
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating workout with ID: {id}");
                return StatusCode(500, $"Failed to update workout: {ex.Message}");
            }
        }

        // POST: api/Workouts
        [HttpPost]
        public async Task<ActionResult<Workout>> CreateWorkout([FromBody] Workout workout)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Add the new workout to the database
                _context.Workout.Add(workout);
                await _context.SaveChangesAsync();

                // Ensure each exercise is associated with the workout and set WorkoutId
                foreach (var exercise in workout.Exercises)
                {
                    exercise.WorkoutId = workout.WorkoutId; // Set WorkoutId correctly
                    _context.Exercises.Add(exercise);
                }

                await _context.SaveChangesAsync();

                return CreatedAtRoute("GetWorkout", new { id = workout.WorkoutId }, workout); // Return 201 Created
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workout.");
                return StatusCode(500, $"Failed to create workout: {ex.Message}");
            }
        }

        // DELETE: api/Workouts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            // Find the workout by ID
            var workout = await _context.Workout.FindAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            // Remove the workout from the database
            _context.Workout.Remove(workout);
            await _context.SaveChangesAsync();

            return NoContent(); // HTTP 204
        }

        private bool WorkoutExists(int id)
        {
            // Check if any workout exists with the specified ID
            return _context.Workout.Any(e => e.WorkoutId == id);
        }
    }
}
