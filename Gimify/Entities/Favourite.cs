namespace Gimify.Entities
{
    public class Favourite : BaseEntity
    {
        public override int id 
            {
            get 
            {
                return UserId;
            }
            set
            {

            }

        }
        public Posts Posts { get; set; }
        public int UserId { get; set; }
        public int Postsid { get; set; }
    }
}
