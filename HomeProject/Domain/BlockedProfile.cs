namespace Domain
{
    public class BlockedProfile
    {
        public int BlockedProfileId { get; set; }
        
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string BProfileId { get; set; }
        public Profile BProfile { get; set; }
    }
}