﻿using Microsoft.AspNetCore.Mvc;
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

        // GET: api/Workouts/Search/{WorkoutName}
        [HttpGet("Search/{workoutName}")]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkoutsByName(string workoutName)
        {
            if (string.IsNullOrWhiteSpace(workoutName))
            {
                return BadRequest("Workout name cannot be empty.");
            }

            try
            {
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

                return Ok(jsonWorkouts);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

            try
            {
                _context.Workout.Add(workout);
                await _context.SaveChangesAsync();

                // Ensure each exercise is associated with the workout and does not set Id
                foreach (var exercise in workout.Exercises)
                {
                    exercise.WorkoutId = workout.WorkoutId; // Ensure WorkoutId is set correctly
                    _context.Exercises.Add(exercise);
                }

                await _context.SaveChangesAsync();

                return CreatedAtRoute("GetWorkout", new { id = workout.WorkoutId }, workout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workout.");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
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