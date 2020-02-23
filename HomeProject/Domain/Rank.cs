using System;
using System.Collections.Generic;
using DAL.Base;

namespace Domain
{
    public class Rank: DomainEntity
    {
        public string RankTitle { get; set; }
        public string RankDescription { get; set; }
        
        public ICollection<ProfileRank> ProfileRanks { get; set; }
    }
}