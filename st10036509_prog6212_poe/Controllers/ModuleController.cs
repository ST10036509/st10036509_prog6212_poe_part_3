using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using st10036509_prog6212_poe.Repositories;
using System.Globalization;

namespace st10036509_prog6212_poe.Controllers
{
    public class ModuleController : Controller
    {
        //cretae a module repository object
        private ModuleRepository _repository = new ModuleRepository();

        // GET: /<controller>/
        [HttpGet]
        //create action method
        public IActionResult ModuleCreation(int userID, string semesterName, int numberOfWeeks, string startDate)
        {
            //create list to store modules
            List<ModuleModel> modules = ModuleRepository.GetModules();
            //pass values to view
            ViewBag.Modules = modules;
            ViewBag.UserID = userID;
            ViewBag.SemesterName = semesterName;
            ViewBag.NumberOfWeeks = numberOfWeeks; 
            ViewBag.StartDate = startDate;
            //return view with list of modules
            return View();
        }//end ModuleCreation method

        // POST: /<controller>/
        [HttpPost] 
        //create action method
        public IActionResult ModuleCreation(string moduleName, string moduleCode, double numberOfCredits, double hoursPerWeek, int userID, string semesterName, int numberOfWeeks, string startDate)
        {
            //create module object
            ModuleModel newModule = new ModuleModel
            {
                ModuleName = moduleName,
                ModuleCode = moduleCode,
                Credits = numberOfCredits,
                ClassHours = hoursPerWeek,
                CompletedHours = new Dictionary<string, double>()
            };

            //add module to list
            ModuleRepository.AddModules(newModule);
            //pass values to view
            ViewBag.Modules = ModuleRepository.GetModules();
            ViewBag.UserID = userID;
            ViewBag.SemesterName = semesterName;
            ViewBag.NumberOfWeeks = numberOfWeeks;
            ViewBag.StartDate = startDate;
            //return view with list of modules
            TempData["AlertMessage"] = "Added Module Successfully!";
            return View();
        }//end ModuleCreation method
    }
}
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________