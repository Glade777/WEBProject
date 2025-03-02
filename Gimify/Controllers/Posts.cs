using Gimify.DAL;
using Gimify.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Entities.Posts posts)
        {
           


                _context.Posts.Add(posts);
                _context.SaveChanges();

                return RedirectToAction("Index");
            
            
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

            return RedirectToAction("Index"); // Перенаправлення на головну сторінку після видалення
        }

        

    }
}
