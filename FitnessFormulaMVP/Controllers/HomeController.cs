using FitnessFormulaMVP.Models;  // Import ErrorViewModel from the Models namespace
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessFormulaMVP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;  // Logger instance for logging

        public HomeController(ILogger<HomeController> logger)  // Constructor injecting ILogger instance
        {
            _logger = logger;  // Assign logger instance
        }

        public IActionResult Index()  // Action method for returning Index view
        {
            return View();  // Return Index view
        }

        public IActionResult Dashboard()  // Action method for returning Dashboard view
        {
            return View();  // Return Dashboard view
        }

        public IActionResult Settings()  // Action method for returning Settings view
        {
            return View();  // Return Settings view
        }

        public IActionResult Index_LP()  // Action method for returning Index_LP view
        {
            return View();  // Return Index_LP view
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]  // Configure ResponseCache attributes
        public IActionResult Error()  // Action method for returning Error view
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });  // Return Error view with ErrorViewModel
        }
    }
}
