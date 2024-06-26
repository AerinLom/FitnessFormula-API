using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FitnessFormulaMVP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FitnessFormulaMVP.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7076/api/";

        public WorkoutController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult NoWorkoutsFound()
        {
            return View("NoWorkoutsFound");
        }

        [HttpGet]
        public IActionResult CreateWorkout()
        {
            return View();
        }
        public async Task<IActionResult> StrengthView()
        {
            try
            {
                var response = await _httpClient.GetAsync("Workouts/category/Strength");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Clean up JSON string by unescaping it and trimming excess characters
                var cleanedContent = Regex.Unescape(content).Trim('"').Replace("\\", "");

                // Deserialize JSON string into List<WorkoutModel> using custom converter
                var workouts = JsonConvert.DeserializeObject<List<WorkoutModel>>(cleanedContent, new CustomWorkoutConverter());

                return View(workouts);
            }
            catch (HttpRequestException)
            {
                // Handle HTTP request errors
                return View("Error");
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors
                return View("Error");
            }
            catch (Exception)
            {
                // Handle other unexpected errors
                return View("Error");
            }
        }

        public async Task<IActionResult> HIITView()
        {
            try
            {
                var response = await _httpClient.GetAsync("Workouts/category/HIIT");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Clean up JSON string by unescaping it and trimming excess characters
                var cleanedContent = Regex.Unescape(content).Trim('"').Replace("\\", "");

                // Deserialize JSON string into List<WorkoutModel> using custom converter
                var workouts = JsonConvert.DeserializeObject<List<WorkoutModel>>(cleanedContent, new CustomWorkoutConverter());

                return View(workouts);
            }
            catch (HttpRequestException)
            {
                // Handle HTTP request errors
                return View("Error");
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors
                return View("Error");
            }
            catch (Exception)
            {
                // Handle other unexpected errors
                return View("Error");
            }
        }

        public async Task<IActionResult> FlexibilityView()
        {
            try
            {
                var response = await _httpClient.GetAsync("Workouts/category/Flexibility");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Clean up JSON string by unescaping it and trimming excess characters
                var cleanedContent = Regex.Unescape(content).Trim('"').Replace("\\", "");

                // Deserialize JSON string into List<WorkoutModel> using custom converter
                var workouts = JsonConvert.DeserializeObject<List<WorkoutModel>>(cleanedContent, new CustomWorkoutConverter());

                return View(workouts);
            }
            catch (HttpRequestException)
            {
                // Handle HTTP request errors
                return View("Error");
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors
                return View("Error");
            }
            catch (Exception)
            {
                // Handle other unexpected errors
                return View("Error");
            }
        }

        public async Task<IActionResult> FunctionalView()
        {
            try
            {
                var response = await _httpClient.GetAsync("Workouts/category/Functional");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Clean up JSON string by unescaping it and trimming excess characters
                var cleanedContent = Regex.Unescape(content).Trim('"').Replace("\\", "");

                // Deserialize JSON string into List<WorkoutModel> using custom converter
                var workouts = JsonConvert.DeserializeObject<List<WorkoutModel>>(cleanedContent, new CustomWorkoutConverter());

                return View(workouts);
            }
            catch (HttpRequestException)
            {
                // Handle HTTP request errors
                return View("Error");
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors
                return View("Error");
            }
            catch (Exception)
            {
                // Handle other unexpected errors
                return View("Error");
            }
        }

        public async Task<IActionResult> ViewWorkout(string name)
        {
            try
            {
                var encodedName = Uri.EscapeDataString(name); // Ensure name is URL-safe

                var response = await _httpClient.GetAsync($"Workouts/name/{name}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Clean up JSON string by unescaping it and trimming excess characters
                var cleanedContent = Regex.Unescape(content).Trim('"').Replace("\\", "");

                // Deserialize JSON string into WorkoutModel
                var workout = JsonConvert.DeserializeObject<WorkoutModel>(cleanedContent, new CustomWorkoutConverter());

                return View(workout);
            }
            catch (HttpRequestException)
            {
                // Handle HTTP request errors
                return View("Error");
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors
                return View("Error");
            }
            catch (Exception)
            {
                // Handle other unexpected errors
                return View("Error");
            }
        }

        public async Task<IActionResult> ViewMyWorkout(string name)
        {
            try
            {
                var encodedName = Uri.EscapeDataString(name); // Ensure name is URL-safe

                var response = await _httpClient.GetAsync($"Workouts/name/{name}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Clean up JSON string by unescaping it and trimming excess characters
                var cleanedContent = Regex.Unescape(content).Trim('"').Replace("\\", "");

                // Deserialize JSON string into WorkoutModel
                var workout = JsonConvert.DeserializeObject<WorkoutModel>(cleanedContent, new CustomWorkoutConverter());

                return View(workout);
            }
            catch (HttpRequestException)
            {
                // Handle HTTP request errors
                return View("Error");
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors
                return View("Error");
            }
            catch (Exception)
            {
                // Handle other unexpected errors
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchWorkouts(string workoutName)
        {
            if (string.IsNullOrWhiteSpace(workoutName))
            {
                return BadRequest("Search query 'workoutName' cannot be empty.");
            }

            try
            {
                // Send GET request to the API endpoint with the workoutName parameter
                var response = await _httpClient.GetAsync($"/api/Workouts/Search/{Uri.EscapeDataString(workoutName)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var cleanedContent = Regex.Unescape(content).Trim('"').Replace("\\", "");
                var workouts = JsonConvert.DeserializeObject<List<WorkoutModel>>(cleanedContent, new CustomWorkoutConverter());

                if (workouts.Count == 0)
                {
                    // No workouts found, return a specific view
                    return RedirectToAction("NoWorkoutsFound");
                }

                return View(workouts);
            }
            catch (HttpRequestException)
            {
                // Handle HTTP request errors
                return RedirectToAction("NoWorkoutsFound");
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors
                return RedirectToAction("NoWorkoutsFound");
            }
            catch (Exception)
            {
                // Handle other unexpected errors
                return RedirectToAction("NoWorkoutsFound");
            }
        }



        public async Task<IActionResult> MyWorkouts()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Home"); // Redirect to login if not authenticated
            }

            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                var response = await _httpClient.GetAsync($"UserWorkouts/MyWorkouts?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Log the raw JSON content for debugging purposes
                    Console.WriteLine("Raw JSON response:");
                    Console.WriteLine(content);

                    // Deserialize JSON string into List<WorkoutModel>
                    var workouts = JsonConvert.DeserializeObject<List<WorkoutModel>>(content);

                    return View(workouts); // Return view with the list of workouts
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Handle case where no workouts were found for the user
                    return View("NoWorkoutsFound");
                }
                else
                {
                    // Handle other HTTP error cases
                    return View("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors
                Console.WriteLine($"HTTP request error: {ex.Message}");
                return View("Error");
            }
            catch (JsonException ex)
            {
                // Handle JSON parsing errors
                Console.WriteLine($"JSON parsing error: {ex.Message}");
                return View("Error");
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return View("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveWorkout(int workoutId)
        {
            // Retrieve userId from session or wherever it's stored
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); // Redirect to login if user is not authenticated
            }

            try
            {
                // Prepare UserWorkout object to send in the POST request
                var userWorkout = new
                {
                    userId = int.Parse(userId),
                    workoutId = workoutId
                };

                // Serialize userWorkout into JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(userWorkout), Encoding.UTF8, "application/json");

                // Send POST request to the API endpoint
                var apiURL = "https://localhost:7076/api/UserWorkouts";
                var response = await _httpClient.PostAsync(apiURL, jsonContent);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Workout saved successfully!";
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Failed to save workout: {errorResponse}";
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["Error"] = $"Failed to save workout: {ex.Message}";
                // Log the exception for debugging purposes
 
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to save workout: {ex.Message}";
;
            }
            return RedirectToAction("Dashboard", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> CreateWorkout(WorkoutModel workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var json = JsonConvert.SerializeObject(workout);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:7076/api/Workouts", content);

                return RedirectToAction("Dashboard", "Home");
            }
            catch (HttpRequestException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


    }
}