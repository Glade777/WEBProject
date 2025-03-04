using System.ComponentModel.DataAnnotations;

namespace Gimify.Entities
{

    public class Admin : User
    {
        
        
        public string AdminUsername { get; private set; } 
        public string AdminPassword { get; private set; } 

    }




    public abstract class BaseEntity
    {
        
        public abstract int id { get; set; }
        public void getInfo() { id = 10;  }
        public virtual void setInfo(int newid) 
        { 
            id = newid; 
        } 
    }

    public class User : BaseEntity
    {
       
        public override int id { get; set; }

        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public string Role { get; set; } = "User";

        public override void setInfo(int newid)
        {
            base.setInfo(newid);
            Console.WriteLine(newid);
        }
    }
}
