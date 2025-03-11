using Gimify.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gimify.DAL.Repositories
{
    public class PostRepository
    {
        private readonly Efcontext _context;

        public PostRepository(Efcontext context)
        {
            _context = context;
        }

        public async Task<List<Posts>> GetPostsSortedByFavouriteCountAsync()
        {
            var posts = await _context.Posts
                                       .OrderByDescending(p => p.FavouriteCount)  
                                       .ToListAsync();

            return posts;
        }

   
        public async Task AddPostAsync(Posts post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        
        public async Task<List<Posts>> GetAllPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Posts?> GetPostByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        
        public async Task UpdatePostAsync(Posts post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        
        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
