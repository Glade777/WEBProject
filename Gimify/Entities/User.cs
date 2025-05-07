using System.ComponentModel.DataAnnotations;

namespace Gimify.Entities
{
    public abstract class BaseEntity
    {
        public virtual int id { get; set; }
    }

    public class User : BaseEntity
    {
        public override int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class Admin : User
    {
        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
        public string Role { get; set; }
    }
}
