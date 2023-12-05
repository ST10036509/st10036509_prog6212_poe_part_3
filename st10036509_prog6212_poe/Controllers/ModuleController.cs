using Microsoft.AspNetCore.Mvc;

namespace st10036509_prog6212_poe.Controllers
{
    public class ModuleController : Controller
    {
        public IActionResult ModuleCreation(int userID)
        {
            ViewBag.UserID = userID;
            return View();
        }
    }
}
