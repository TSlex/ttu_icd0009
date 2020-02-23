namespace Domain
{
    public class Follower
    {
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string FollowerProfileId { get; set; }
        public Profile FollowerProfile { get; set; }
    }
}