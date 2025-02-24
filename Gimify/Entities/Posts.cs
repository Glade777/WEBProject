namespace Gimify.Entities
{
    public class PostsMaker
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float CreatedAt { get; set; }

        public List<Favourite> Favourite { get; set; }

    }
}
