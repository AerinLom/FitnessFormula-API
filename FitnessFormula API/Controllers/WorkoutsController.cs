using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessFormula_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessFormula_API.Data;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FitnessFormula_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Workouts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
        {
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

            return Ok(jsonWorkouts);
        }

        // GET: api/Workouts/name/{name}
        [HttpGet("name/{name}")]
        public async Task<ActionResult<Workout>> GetWorkoutByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Workout name cannot be empty.");
            }

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

            return Ok(jsonWorkout);
        }


        // GET: api/Workouts/category/Strength
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkoutsByCategory(string category)
        {
            var workouts = await _context.Workout
                .Include(w => w.Exercises)
                .Where(w => w.Category == category)
                .ToListAsync();

            if (workouts == null || workouts.Count == 0)
            {
                return NotFound();
            }

            // Configure JsonSerializerOptions with ReferenceHandler.Preserve
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            // Serialize workouts with the configured options
            var jsonWorkouts = JsonSerializer.Serialize(workouts, options);

            return Ok(jsonWorkouts);
        }

        // GET: api/Workouts/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Workout>>> SearchWorkoutsByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search query 'name' cannot be empty.");
            }

            var workouts = await _context.Workout
                .Include(w => w.Exercises) // Include if you have related entities
                .Where(w => w.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (workouts == null || workouts.Count == 0)
            {
                return NotFound("No workouts found.");
            }

            return Ok(workouts);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, [FromBody] Workout updatedWorkout)
        {
            if (id != updatedWorkout.WorkoutId)
            {
                return BadRequest();
            }

            var existingWorkout = await _context.Workout
                .Include(w => w.Exercises) // Ensure exercises are loaded
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

            try
            {
                // Update the workout in the database
                _context.Entry(existingWorkout).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Workout>> CreateWorkout([FromBody] Workout workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add the workout object to the context's Workout DbSet
            _context.Workout.Add(workout);

            // Ensure each exercise is associated with the workout
            foreach (var exercise in workout.Exercises)
            {
                exercise.WorkoutId = workout.WorkoutId;
                _context.Exercises.Add(exercise);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Optional: Create a minimal workout object for response
            var responseWorkout = new Workout
            {
                WorkoutId = workout.WorkoutId,
                Name = workout.Name
            };

            return CreatedAtRoute("GetWorkout", new { id = workout.WorkoutId }, responseWorkout);
        }



        // DELETE: api/Workouts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var workout = await _context.Workout.FindAsync(id); // Singular "Workout"
            if (workout == null)
            {
                return NotFound();
            }

            _context.Workout.Remove(workout); // Singular "Workout"
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkoutExists(int id)
        {
            return _context.Workout.Any(e => e.WorkoutId == id); // Singular "Workout"
        }
    }
}