using System.ComponentModel.DataAnnotations;

namespace Gimify.Entities
{
    public class Posts
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
      
    
        public List<Favourite> Favourite { get; set; }

    }
}
