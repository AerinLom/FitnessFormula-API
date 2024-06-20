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
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserProfile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfiles()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        // GET: api/UserProfile/ById/{userId}
        [HttpGet("Id/{userId}")]
        public async Task<ActionResult<UserProfile>> GetUserProfileByID(int userId)
        {
            try
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == userId);

                if (userProfile == null)
                {
                    return NotFound(); // Return 404 if userProfile with specified userId is not found
                }

                return Ok(userProfile); // Return userProfile if found
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // Example: _logger.LogError(ex, $"Error occurred while fetching UserProfile with userId {userId}");

                return StatusCode(500, "An error occurred while fetching the user profile."); // Return 500 Internal Server Error for other exceptions
            }
        }

        // GET: api/UserProfile/ByUsername/{username}
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<UserProfile>> GetUserProfileByUsername(string username)
        {
            try
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Username == username);

                if (userProfile == null)
                {
                    return NotFound(); // Return 404 if userProfile with specified username is not found
                }

                return Ok(userProfile); // Return userProfile if found
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // Example: _logger.LogError(ex, $"Error occurred while fetching UserProfile with username {username}");

                return StatusCode(500, "An error occurred while fetching the user profile."); // Return 500 Internal Server Error for other exceptions
            }
        }

        // PUT: api/UserProfile/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(int id, UserProfileInputModel updatedUserProfile)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound("UserProfile not found.");
            }

            // Update fields based on the provided updatedUserProfile
            if (updatedUserProfile.Username != null)
            {
                userProfile.Username = updatedUserProfile.Username;
            }
            if (updatedUserProfile.Password != null)
            {
                userProfile.Password = updatedUserProfile.Password;
            }
            if (updatedUserProfile.Email != null)
            {
                userProfile.Email = updatedUserProfile.Email;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
                {
                    return NotFound("UserProfile not found.");
                }
                else
                {
                    throw; // This will bubble up the exception for logging purposes
                }
            }

            return NoContent(); // HTTP 204
        }



        // POST: api/UserProfile
        [HttpPost("UserProfile")]
        public async Task<IActionResult> PostUserProfile([FromBody] UserProfileInputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Assuming you're using Entity Framework Core for database operations
                // Insert logic here, ensuring proper mapping and handling of model properties
                _context.UserProfiles.Add(new UserProfile
                {
                    UserId = model.UserId,  // Ensure UserId is correctly assigned
                    Username = model.Username,
                    Password = model.Password,
                    Email = model.Email
                    // Other properties as needed
                });

                await _context.SaveChangesAsync();

                return Ok(); // or return CreatedAtAction() if you want to return more details
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // Example: _logger.LogError(ex, "Error occurred while saving UserProfile");

                return StatusCode(500, "An error occurred while saving the user profile.");
            }
        }

        // DELETE: api/UserProfile/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(e => e.UserId == id);
        }
    }
}
