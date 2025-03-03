using System.ComponentModel.DataAnnotations;

namespace Gimify.Entities
{
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

        public override void setInfo(int newid)
        {
            base.setInfo(newid);
            Console.WriteLine(newid);
        }
    }
}
