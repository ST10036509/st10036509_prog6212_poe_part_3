using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using st10036509_prog6212_poe.Repositories;

namespace st10036509_prog6212_poe.Controllers
{
    public class SemesterController : Controller
    {
        List<ModuleModel> modulesList = new List<ModuleModel>();

        [HttpGet]
        public IActionResult SemesterCreation(int userID, List<ModuleModel> modules = null, 
            string semesterName = null, int numberOfWeeks = 0, string startDate = null)
        {
            modulesList = modules;
            ViewBag.Modules = modulesList;
            ViewBag.UserID = userID;
            ViewBag.SemesterName = semesterName;
            ViewBag.NumberOfWeeks = numberOfWeeks;
            ViewBag.StartDate = startDate;
            return View();
        }
    }
}
