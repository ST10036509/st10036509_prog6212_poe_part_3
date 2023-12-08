using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using st10036509_prog6212_poe.Repositories;
using System.Diagnostics;

namespace st10036509_prog6212_poe.Controllers
{
    public class HomeController : Controller
    {
        //declare variables
        private readonly ILogger<HomeController> _logger;
        //create constructor for logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /<controller>/
        [HttpGet]
        //create action method
        public IActionResult About(int userID)
        {
            //create list to store modules
            ModuleRepository.ClearModules();
            //create list to store users
            ViewBag.UserID = userID;
            //return view with list of modules
            return View();
        }//end About method
    }
}
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________