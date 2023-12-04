using Microsoft.AspNetCore.Mvc;

namespace st10036509_prog6212_poe.Controllers
{
    public class PlannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserPlanner(string username)
        {
            ViewBag.Username = username;
            return View();
        }
    }
}
