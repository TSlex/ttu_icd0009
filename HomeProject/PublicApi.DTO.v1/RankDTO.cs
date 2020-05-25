using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    /// <summary>
    /// Get only
    /// </summary>
    public class RankDTO
    {
        public string RankTitle { get; set; } = default!;
        public string? RankDescription { get; set; }

        public string? RankIcon { get; set; }

        public string RankColor { get; set; } = default!;
        public string RankTextColor { get; set; } = default!;

        public int MaxExperience { get; set; } = default!;
        public int MinExperience { get; set; } = default!;
    }

    public class RankAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)] public string RankCode { get; set; } = default!;

        public Guid RankTitleId { get; set; } = default!;
        public string? RankTitle { get; set; } = default!;
        public Guid? RankDescriptionId { get; set; }
        public string? RankDescription { get; set; }

        [MaxLength(20)] public string RankColor { get; set; } = default!;
        [MaxLength(20)] public string RankTextColor { get; set; } = default!;
        [MaxLength(20)] public string? RankIcon { get; set; }

        public int MaxExperience { get; set; } = default!;
        public int MinExperience { get; set; } = default!;

        public Guid? PreviousRankId { get; set; }

        public Guid? NextRankId { get; set; }
    }
}