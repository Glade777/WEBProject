using Gimify.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gimify.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDataStorage<User> _userStorage;
        private readonly IDataStorage<Admin> _adminStorage;

        public AccountController(IDataStorage<User> userStorage, IDataStorage<Admin> adminStorage)
        {
            _userStorage = userStorage;
            _adminStorage = adminStorage;
        }

        
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            var user = _userStorage.GetAll().FirstOrDefault(u => u.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Posts");
            }

            var admin = _adminStorage.GetAll().FirstOrDefault(a => a.AdminUsername == username);
            if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.AdminPassword))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.id.ToString()),
                    new Claim(ClaimTypes.Name, admin.AdminUsername),
                    new Claim(ClaimTypes.Role, admin.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Posts");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Posts");
        }

        
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(string username, string password, bool isAdmin = false)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            if (_userStorage.GetAll().Any(u => u.Username == username) || _adminStorage.GetAll().Any(a => a.AdminUsername == username))
            {
                ModelState.AddModelError("", "This username is already taken.");
                return View();
            }

            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                if (isAdmin)
                {
                    var newAdmin = new Admin
                    {
                        AdminUsername = username,
                        AdminPassword = passwordHash,
                        Role = "Admin"
                    };
                    _adminStorage.Add(newAdmin);
                }
                else
                {
                    var newUser = new User
                    {
                        Username = username,
                        Password = passwordHash,
                        Role = "User"
                    };
                    _userStorage.Add(newUser);
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return View();
            }
        }

        
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
