namespace Gimify.Entities
{
    public class User
    {
        public int id { get; set; }
        public virtual string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
    }

    class Derivedclass : User
    {
        public string _username;

        public override string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _username = value;
                }
                else
                {
                    _username = "Unknown";
                }
            }
        }
    }
}
