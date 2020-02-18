using System;
using System.Collections.Generic;

namespace Domain
{
    public class Rank
    {
        public int RankId { get; set; }
        public string RankTitle { get; set; }
        public string RankDescription { get; set; }
        
        public ICollection<ProfileRank> ProfileRanks { get; set; }
    }
}