namespace Domain
{
    public class Follower
    {
        public int FollowerId { get; set; }

        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string FollowerProfileId { get; set; }
        public Profile FollowerProfile { get; set; }
    }
}