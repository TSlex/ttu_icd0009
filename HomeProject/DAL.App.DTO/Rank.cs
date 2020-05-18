using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace DAL.App.DTO
{
    public class Rank: DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)] public string RankTitle { get; set; } = default!;
        [MaxLength(300)] public string? RankDescription { get; set; }
        
        public Guid RankTitleId { get; set; } = default!;
        public Guid? RankDescriptionId { get; set; }
        
        [MaxLength(100)] public string RankCode { get; set; } = default!;

        [MaxLength(20)] public string RankColor { get; set; } = default!;
        [MaxLength(20)] public string RankTextColor { get; set; } = default!;
        [MaxLength(20)] public string? RankIcon { get; set; }


        public int MaxExperience { get; set; } = default!;
        public int MinExperience { get; set; } = default!;
        
        public Guid? PreviousRankId { get; set; }
        public Rank? PreviousRank { get; set; }
        
        public Guid? NextRankId { get; set; }
        public Rank? NextRank { get; set; }

        public ICollection<ProfileRank>? ProfileRanks { get; set; }
    }
}