using Microsoft.AspNetCore.Mvc;
using StellarMinds.Models;
using System.Diagnostics;

namespace StellarMinds.Controllers
{
    public class HomeController : Controller
    {
        private string? Token => HttpContext.Session.GetString("token");

        public IActionResult Index()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            return View();
        }

        public IActionResult Privacy()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
