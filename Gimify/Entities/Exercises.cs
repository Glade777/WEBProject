namespace Gimify.Entities
{
    public class Exercises
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public List<Exercises_Man> Exercises_Man { get; set; }
    }
}
