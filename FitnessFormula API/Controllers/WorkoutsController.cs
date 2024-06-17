using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessFormula_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessFormula_API.Data;

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
            return await _context.Workout.ToListAsync(); // Singular "Workout"
        }

        // GET: api/Workouts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Workout>> GetWorkout(int id)
        {
            var workout = await _context.Workout.FindAsync(id); // Singular "Workout"

            if (workout == null)
            {
                return NotFound();
            }

            return workout;
        }

        // PUT: api/Workouts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkout(int id, Workout updatedWorkout)
        {
            if (id != updatedWorkout.WorkoutId)
            {
                return BadRequest();
            }

            var workout = await _context.Workout.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }

            // Update only the fields that are provided in the request body
            if (!string.IsNullOrEmpty(updatedWorkout.Name))
            {
                workout.Name = updatedWorkout.Name;
            }
            if (!string.IsNullOrEmpty(updatedWorkout.Category))
            {
                workout.Category = updatedWorkout.Category;
            }
            if (!string.IsNullOrEmpty(updatedWorkout.Difficulty))
            {
                workout.Difficulty = updatedWorkout.Difficulty;
            }
            if (updatedWorkout.Duration > 0)
            {
                workout.Duration = updatedWorkout.Duration;
            }
            if (!string.IsNullOrEmpty(updatedWorkout.Exercises))
            {
                workout.Exercises = updatedWorkout.Exercises;
            }

            _context.Entry(workout).State = EntityState.Modified;

            try
            {
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

        // POST: api/Workouts
        [HttpPost]
        public async Task<ActionResult<Workout>> PostWorkout(Workout workout)
        {
            _context.Workout.Add(workout); // Singular "Workout"
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkout), new { id = workout.WorkoutId }, workout);
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