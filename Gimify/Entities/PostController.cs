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

    }
}
