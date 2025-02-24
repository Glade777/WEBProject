namespace Gimify.Entities
{
    public class User
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
