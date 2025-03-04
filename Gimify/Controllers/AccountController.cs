using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;
using Gimify.DAL;
using Gimify.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gimify.Controllers
{
    public class AccountController : Controller
    {
        private readonly Efcontext _context;

        public AccountController(Efcontext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                try
                {
                    if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                        return RedirectToAction("Index", "Posts");
                    }
                }
                catch (BCrypt.Net.SaltParseException)
                {
                    // Handle invalid salt version by re-hashing the password
                    string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                    user.Password = newPasswordHash;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Posts");
                }
            }

            ModelState.AddModelError(string.Empty, "Невірний логін або пароль");
            return View();
        }


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
        public async Task<IActionResult> Register(string username, string password)
        {
            if (_context.User.Any(u => u.Username == username))
            {
                ModelState.AddModelError("", "Цей логін вже зайнятий.");
                return View();
            }

            // Hash the password before storing it
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                Password = passwordHash 
            };

            
            if (user.id == 0) 
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                
                ModelState.AddModelError("", "Помилка з ідентифікатором користувача.");
                return View();
            }

            return RedirectToAction("Login");
        }

    }
}
