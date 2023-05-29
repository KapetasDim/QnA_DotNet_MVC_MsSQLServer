using QnA_DotNet_MVC_MsSQLServer.Data;
using QnA_DotNet_MVC_MsSQLServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ManagementProject.Controllers
{
   
    public class LoginController : Controller
    {
        private readonly QnA_DotNet_MVC_MsSQLServer_DBContext db = new QnA_DotNet_MVC_MsSQLServer_DBContext();

        public IActionResult Index()
        {
            return RedirectToAction("Login" , "Login"/*this is the controller*/);
        }
        public IActionResult Login()
        {

            if (HttpContext.Session.GetString("userSession") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserTable user)
        {
            var currentUser = db.UserTables.Where(u => u.Username == user.Username && u.Password == user.Password).ToList();
            if (currentUser.Count == 1)
            {
                HttpContext.Session.SetString("userSession", user.Username.ToString());

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Incorrect credentials";
                return View("Login");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userSession");
            return RedirectToAction("Login");
        }
    }
}
