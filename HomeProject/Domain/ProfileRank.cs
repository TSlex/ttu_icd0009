namespace Domain
{
    public class ProfileRank
    {
        public int ProfileRankId { get; set; }
        
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string RankId { get; set; }
        public Rank Rank { get; set; }
    }
}