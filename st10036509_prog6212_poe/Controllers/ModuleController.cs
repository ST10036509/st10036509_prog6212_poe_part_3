using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;

namespace st10036509_prog6212_poe.Controllers
{
    public class ModuleController : Controller
    {
        [HttpGet]
        public IActionResult ModuleCreation(int userID, List<ModuleModel> modules = null)
        {
            ViewBag.Modules = modules;
            ViewBag.UserID = userID;
            return View();
        }

        [HttpPost] 
        public IActionResult ModuleCreation(string moduleName, string moduleCode, double numberOfCredits, double hoursPerWeek, int userID, List<ModuleModel> modules = null)
        {
            ViewBag.UserID = userID;
            ViewBag.Modules = modules;

            modules.Add(new ModuleModel
            {
                ModuleName = moduleName,
                ModuleCode = moduleCode,
                Credits = numberOfCredits,
                ClassHours = hoursPerWeek
            });

            return View();
        }
    }
}
