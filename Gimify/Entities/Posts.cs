using System.ComponentModel.DataAnnotations;

namespace Gimify.Entities
{
    public class Posts
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public int FavouriteCount { get; set; } = 0;
        public List<Favourite> Favourite { get; set; } = new List<Favourite>();

    }
}
