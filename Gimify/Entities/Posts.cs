using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gimify.Entities
{
    public class Posts : BaseEntity 
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        public string description { get; set; } = string.Empty;

        public int FavouriteCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public string AuthorUsername { get; set; }

        [JsonIgnore] 
        public ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();
    }
}
