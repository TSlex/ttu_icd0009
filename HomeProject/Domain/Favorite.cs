namespace Domain
{
    public class Favorite
    {
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}