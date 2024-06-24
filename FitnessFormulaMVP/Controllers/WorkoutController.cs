// Controllers/WorkoutController.cs

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FitnessFormulaMVP.Models;
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
    }
}