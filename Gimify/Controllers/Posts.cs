using Gimify.DAL;
using Gimify.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Gimify.Controllers
{
    
    public class Posts : Controller
    {
        private readonly Efcontext _context;
        public Posts(Efcontext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var posts = _context.Posts.ToList();
            return View(posts);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Create(Entities.Posts posts)
        {


            if (posts != null)
            {
                _context.Posts.Add(posts);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index");
            }


        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.id == id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }



        [HttpPost]
        public IActionResult Favourite(int id)
        {
            // Отримуємо ID користувача
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(); // Якщо користувач не авторизований
            }

            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId)) // Безпечне перетворення на int
            {
                return Unauthorized(); // Якщо перетворити не вдалося, повертаємо помилку авторизації
            }

            var post = _context.Posts.FirstOrDefault(p => p.id == id);
            if (post == null)
            {
                return NotFound();
            }

            var existingFavourite = _context.Favourite
                .FirstOrDefault(f => f.Postsid == id && f.UserId == userId);

            if (existingFavourite != null)
            {
                _context.Favourite.Remove(existingFavourite);
                post.FavouriteCount = Math.Max(0, post.FavouriteCount - 1);
            }
            else
            {
                var favourite = new Favourite
                {
                    Postsid = id,
                    UserId = userId
                };
                _context.Favourite.Add(favourite);
                post.FavouriteCount += 1;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
    
