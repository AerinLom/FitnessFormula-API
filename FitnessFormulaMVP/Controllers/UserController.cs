using System;  // Import necessary namespaces
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FitnessFormulaMVP.Models;  // Import ErrorViewModel, UserModel, and LoginModel from the Models namespace

namespace FitnessFormulaMVP.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;  // HTTP client instance for making requests to the API
        private readonly string _baseUrl = "https://localhost:7076/api/";  // Base URL of the API

        public UserController()  // Constructor initializing HttpClient and setting base address and headers
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Register()  // Action method for displaying registration view
        {
            return View();  // Return Register view
        }

        public IActionResult Login()  // Action method for displaying login view
        {
            return View();  // Return Login view
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)  // POST action method for user registration
        {
            if (ModelState.IsValid)  // Validate model state
            {
                try
                {
                    // Serialize the UserModel object to JSON
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Send the POST request to the external API
                    var response = await _httpClient.PostAsync("UserProfile/UserProfile", content);

                    // Check for successful response
                    response.EnsureSuccessStatusCode(); // Throws on error status

                    // Redirect to the Home/Index action after successful registration
                    return RedirectToAction("Index", "Home");
                }
                catch (HttpRequestException ex)
                {
                    // Log or handle the exception (e.g., return error view)
                    // Consider logging the exception details for debugging purposes.
                    return View("Error", new ErrorViewModel { Message = ex.Message }); // Pass error message to Error view
                }
            }

            // Return validation error or bad request
            return BadRequest(new { success = false, message = "Invalid input" });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)  // POST action method for user login
        {
            if (ModelState.IsValid)  // Validate model state
            {
                try
                {
                    var username = model.Username;
                    var response = await _httpClient.GetAsync($"UserProfile/Username/{username}");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var userProfile = JsonConvert.DeserializeObject<UserModel>(content);

                    if (userProfile != null && userProfile.Password == model.Password)
                    {
                        // Login successful
                        TempData["Username"] = userProfile.Username; // Store username in TempData
                        HttpContext.Session.SetString("Username", userProfile.Username);
                        HttpContext.Session.SetString("UserId", userProfile.UserId.ToString());
                        TempData["SuccessMessage"] = "Login successful! Welcome to Fitness Formula.";
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                        return View(model); // Return login view with error message
                    }
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError("", "Error while logging in. Please try again later.");
                    return View(); // Return login view with error message
                }
            }

            // If ModelState is not valid, return the login view with validation errors
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        public async Task<IActionResult> Settings()  // Action method for displaying user settings
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                // Handle case where username is not found in session
                return RedirectToAction("Index", "Home"); // Redirect to home or login page
            }

            try
            {
                var response = await _httpClient.GetAsync($"UserProfile/Username/{username}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var userProfile = JsonConvert.DeserializeObject<UserModel>(content);

                return View(userProfile); ; // Pass user details to the settings view
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("", "Error while retrieving user details. Please try again later.");
                return View(); // Return view with error message
            }
        }

        public async Task<IActionResult> Logout()  // Action method for logging out user
        {
            return RedirectToAction("Index", "Home");  // Redirect to Home/Index action
        }
    }
}
