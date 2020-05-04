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
}