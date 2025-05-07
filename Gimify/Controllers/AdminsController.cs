using Microsoft.AspNetCore.Mvc;
using Gimify.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Gimify.Controllers
{
    
    public class AdminsController : Controller
    {
        private readonly IDataStorage<Admin> _adminStorage;

        public AdminsController(IDataStorage<Admin> adminStorage)
        {
            _adminStorage = adminStorage;
        }

        public IActionResult Index()
        {
            var admins = _adminStorage.GetAll();
            return View(admins);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admin = _adminStorage.GetAll().FirstOrDefault(a => a.Username == username);

            if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, admin.id.ToString()),
            new Claim(ClaimTypes.Name, admin.Username),
            new Claim(ClaimTypes.Role, admin.Role ?? "Admin")
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Posts");
            }

            ModelState.AddModelError("", "Невірний логін або пароль.");
            return View();
        }





        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        public IActionResult account()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Create(Admin admin)
        {
            if (string.IsNullOrEmpty(admin.AdminUsername) || string.IsNullOrEmpty(admin.AdminPassword))
            {
                ModelState.AddModelError("", "Username and Password must be provided.");
                return View();
            }

            admin.AdminPassword = BCrypt.Net.BCrypt.HashPassword(admin.AdminPassword);

            admin.Username = admin.AdminUsername; 

            _adminStorage.Add(admin);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var admin = _adminStorage.GetById(id);
            if (admin != null)
            {
                _adminStorage.Delete(id); 
                return RedirectToAction("Index");  
            }

            return NotFound(); 
        }
    }
}
