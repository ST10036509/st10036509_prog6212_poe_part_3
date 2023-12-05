using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;

namespace st10036509_prog6212_poe.Controllers
{
    public class SemesterController : Controller
    {

        [HttpGet]
        public IActionResult SemesterCreation(int userID, List<ModuleModel> modules = null)
        {
            ViewBag.Modules = modules;
            ViewBag.UserID = userID;
            return View();
        }
    }
}
