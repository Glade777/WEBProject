using System.ComponentModel.DataAnnotations;

namespace Gimify.Entities
{
    public class User
    {
        public int id { get; set; }
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }


}
