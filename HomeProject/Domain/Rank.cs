using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain
{
    public class Rank: DomainEntityBaseMetadata
    {
        [MaxLength(100)] public string RankTitle { get; set; } = default!;
        [MaxLength(100)] public string RankCode { get; set; } = default!;

        [MaxLength(20)] public string RankColor { get; set; } = default!;
        [MaxLength(20)] public string RankTextColor { get; set; } = default!;
        [MaxLength(20)] public string? RankIcon { get; set; }
        
        [MaxLength(300)] public string? RankDescription { get; set; }

        public int MaxExperience { get; set; } = default!;

        [ForeignKey(nameof(Id))]
        public Guid? PreviousRankId { get; set; }
        public Rank? PreviousRank { get; set; }
        
        [ForeignKey(nameof(Id))]
        public Guid? NextRankId { get; set; }
        public Rank? NextRank { get; set; }

        public ICollection<ProfileRank>? ProfileRanks { get; set; }
    }
}