using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using st10036509_prog6212_poe.Repositories;
using System.Globalization;

namespace st10036509_prog6212_poe.Controllers
{
    public class ModuleController : Controller
    {
        private ModuleRepository _repository = new ModuleRepository();

        [HttpGet]
        public IActionResult ModuleCreation(int userID, string semesterName, int numberOfWeeks, string startDate)
        {
            List<ModuleModel> modules = ModuleRepository.GetModules();
            ViewBag.Modules = modules;
            ViewBag.UserID = userID;
            ViewBag.SemesterName = semesterName;
            ViewBag.NumberOfWeeks = numberOfWeeks; 
            ViewBag.StartDate = startDate;
            return View();
        }

        [HttpPost] 
        public IActionResult ModuleCreation(string moduleName, string moduleCode, double numberOfCredits, double hoursPerWeek, int userID)
        {

            ModuleModel newModule = new ModuleModel
            {
                ModuleName = moduleName,
                ModuleCode = moduleCode,
                Credits = numberOfCredits,
                ClassHours = hoursPerWeek
            };

            ModuleRepository.AddModules(newModule);
            ViewBag.Modules = ModuleRepository.GetModules();
            ViewBag.UserID = userID;
            return View();
        }
    }
}
