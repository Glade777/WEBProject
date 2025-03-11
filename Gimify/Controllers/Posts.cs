using Gimify.DAL;
using Gimify.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gimify.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly Efcontext _context;

        public PostsController(Efcontext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["FavouriteSortParm"] = string.IsNullOrEmpty(sortOrder) ? "favourite_desc" : "";

            var posts = from p in _context.Posts
                        select p;

            switch (sortOrder)
            {
                case "favourite_desc":
                    posts = posts.OrderByDescending(p => p.FavouriteCount);
                    break;
                default:
                    posts = posts.OrderBy(p => p.FavouriteCount); 
                    break;
            }

            return View(await posts.ToListAsync());
        }

        // Створення нового поста
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Entities.Posts post)
        {
            if (ModelState.IsValid)
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.id == id);
            if (post == null)
            {
                TempData["Error"] = "Post not found.";
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null && post.UserId.ToString() == currentUserId)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "You do not have permission to delete this post.";
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

       
        [HttpPost]
        public async Task<IActionResult> Favourite(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.id == id);

            if (post == null)
            {
                return NotFound();
            }

            var existingFavourite = await _context.Favourite
                .FirstOrDefaultAsync(f => f.Postsid == id);

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
                    UserId = id
                };
                _context.Favourite.Add(favourite);
                post.FavouriteCount += 1;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
