using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using st10036509_prog6212_poe.Repositories;
using System.Diagnostics;

namespace st10036509_prog6212_poe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult About(int userID)
        {
            ModuleRepository.ClearModules();
            ViewBag.UserID = userID;
            return View();
        }
    }
}