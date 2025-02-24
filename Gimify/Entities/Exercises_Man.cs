namespace Gimify.Entities
{
    public class Exercises_Man
    {
        public int id { get; set; }
        public string name { get; set; }

        public int Exercisesid { get; set; }
        public Exercises Exercises { get; set; }
    }
}
