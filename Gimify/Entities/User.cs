using System.ComponentModel.DataAnnotations;

namespace Gimify.Entities
{
    public class User
    {
        public int id { get; set; }
        public virtual string Username { get; set; }
        [Required]
        public string Password { get; set; }
       
    }


}
