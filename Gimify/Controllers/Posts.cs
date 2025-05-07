using Gimify.BLL.Services;
using Gimify.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

namespace Gimify.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [AllowAnonymous]
        public IActionResult Index(string sortOrder)
        {
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["PopularitySortParam"] = sortOrder == "popularity" ? "popularity_desc" : "popularity";


            var posts = _postService.GetAllPostsWithUsernames();


            posts = sortOrder switch
            {
                "date" => posts.OrderBy(p => p.CreatedAt).ToList(),
                "date_desc" => posts.OrderByDescending(p => p.CreatedAt).ToList(),
                "popularity" => posts.OrderBy(p => p.FavouriteCount).ToList(),
                "popularity_desc" => posts.OrderByDescending(p => p.FavouriteCount).ToList(),
                _ => posts.OrderByDescending(p => p.CreatedAt).ToList()
            };

            return View(posts);
        }

        public IActionResult Create() => View();
        [HttpPost]
        [Authorize]
        public IActionResult Create(Posts post)
        {
            // Валідація на рівні UI
            var validationResults = _postService.ValidateEntity<Posts>(post);
            if (validationResults.Any())
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = validationResults.Select(r => r.ErrorMessage).ToList();
                    return BadRequest(new { Errors = errors });
                }
                foreach (var error in validationResults)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
                return View(post);
            }

            try
            {
                post.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _postService.CreatePost(post);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { Success = true, RedirectUrl = Url.Action("Index", "Posts") });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest(new { Errors = new[] { ex.Message } });
                }
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(post);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return StatusCode(500, new { Errors = new[] { "An unexpected error occurred." } });
                }
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the post.");
                return View(post);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var post = _postService.GetById(id);
                if (post == null) return NotFound();

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (post.UserId.ToString() != currentUserId && !User.IsInRole("Admin"))
                {
                    TempData["Error"] = "You do not have permission to delete this post.";
                    return RedirectToAction(nameof(Index));
                }

                _postService.DeletePost(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id)
        {
            var post = _postService.GetById(id);
            return post == null ? NotFound() : View(post);
        }

        [HttpPost]
        public IActionResult Favourite(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _postService.ToggleFavourite(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult FavouritePosts()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var favouritePosts = _postService.GetFavouritePosts(userId);
            return View(favouritePosts);
        }

        public IActionResult Edit(int id)
        {
            var post = _postService.GetById(id);
            if (post == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId.ToString() != currentUserId && !User.IsInRole("Admin"))
            {
                TempData["Error"] = "You do not have permission to edit this post.";
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Posts post)
        {

            // Валідація на рівні UI
            var validationResults = _postService.ValidateEntity<Posts>(post);
            if (validationResults.Any())
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = validationResults.Select(r => r.ErrorMessage).ToList();
                    return BadRequest(new { Errors = errors });
                }
                foreach (var error in validationResults)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
                return View(post);
            }

            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                post.UserId = int.Parse(currentUserId);
                _postService.UpdatePost(post);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { Success = true, RedirectUrl = Url.Action("Index", "Posts") });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest(new { Errors = new[] { ex.Message } });
                }
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(post);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return StatusCode(500, new { Errors = new[] { "An unexpected error occurred." } });
                }
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the post.");
                return View(post);
            }
        }
    }
}