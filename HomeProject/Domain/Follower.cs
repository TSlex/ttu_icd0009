namespace Domain
{
    public class Follower
    {
        public string FollowerId { get; set; }

        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string FollowerProfileId { get; set; }
        public Profile FollowerProfile { get; set; }
    }
}