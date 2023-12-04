using Microsoft.AspNetCore.Mvc;

namespace st10036509_prog6212_poe.Controllers
{
    public class PlannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserPlanner()
        {
            return View();
        }
    }
}
