using Microsoft.AspNetCore.Mvc;
using QnA_DotNet_MVC_MsSQLServer.Models;
using System.Diagnostics;

namespace QnA_DotNet_MVC_MsSQLServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.userSession = HttpContext.Session.GetString("userSession");
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.userSession = HttpContext.Session.GetString("userSession");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.userSession = HttpContext.Session.GetString("userSession");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}