﻿using Microsoft.AspNetCore.Mvc;

namespace st10036509_prog6212_poe.Controllers
{
    public class SemesterController : Controller
    {
        [HttpGet]
        public IActionResult SemesterCreation(int userID)
        {
            ViewBag.UserID = userID;
            return View();
        }
    }
}
