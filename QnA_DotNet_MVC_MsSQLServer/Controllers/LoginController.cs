using QnA_DotNet_MVC_MsSQLServer.Data;
using QnA_DotNet_MVC_MsSQLServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace ManagementProject.Controllers
{

    public class LoginController : Controller
    {
        private readonly QnA_DotNet_MVC_MsSQLServer_DBContext db = new QnA_DotNet_MVC_MsSQLServer_DBContext();

        public IActionResult Index()
        {
            return RedirectToAction("Login1", "Login"/*this is the controller*/);
        }
        public IActionResult Login1()
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
                return View("Login1");
            }
        }





        public IActionResult CreateUser1()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("Username,Password,FirstName,LastName," +
            "Residence,Email,DateOfBirth")] UserTable user)
        {
            user.AccessLevel = 0;
            user.DateOfDataEdit = DateTime.Now;
            user.DateOfEntry = DateTime.Now;
       


            var currentUser = db.UserTables.Where(u => u.Username == user.Username).ToList();
            if (currentUser.Count != 0)
            {
                ViewBag.Message = "Username Taken";
                return RedirectToAction("CreateUser", "Login");
            }

            db.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Login1", "Login");
        }

















        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userSession");
            return RedirectToAction("Login");
        }

    }
}
