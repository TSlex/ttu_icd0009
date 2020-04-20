using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Rank: DomainEntityBaseMetadata
    {
        [MaxLength(100)] public string RankTitle { get; set; } = default!;
        [MaxLength(300)] public string? RankDescription { get; set; }
        
        public ICollection<ProfileRank>? ProfileRanks { get; set; }
    }
}