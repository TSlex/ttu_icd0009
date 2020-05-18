using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Rank : DomainEntityBaseMetaSoftUpdateDelete
    {
        public Guid RankTitleId { get; set; } = default!;
        public Guid? RankDescriptionId { get; set; }
        
        [Display(Name = nameof(RankTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(100)]
        public string RankTitle { get; set; } = default!;

        [Display(Name = nameof(RankCode), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(100)]
        public string RankCode { get; set; } = default!;

        [Display(Name = nameof(RankColor), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(20)]
        public string RankColor { get; set; } = default!;

        [Display(Name = nameof(RankTextColor), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(20)]
        public string RankTextColor { get; set; } = default!;

        [Display(Name = nameof(RankIcon), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(20)]
        public string? RankIcon { get; set; }

        [Display(Name = nameof(RankDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(300)]
        public string? RankDescription { get; set; }

        [Display(Name = nameof(MaxExperience), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public int MaxExperience { get; set; } = default!;

        [Display(Name = nameof(MinExperience), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public int MinExperience { get; set; } = default!;

        [Display(Name = nameof(PreviousRankId), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public Guid? PreviousRankId { get; set; }

        [Display(Name = nameof(PreviousRank), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public Rank? PreviousRank { get; set; }

        [Display(Name = nameof(NextRankId), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public Guid? NextRankId { get; set; }

        [Display(Name = nameof(NextRank), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public Rank? NextRank { get; set; }

        [Display(Name = nameof(ProfileRanks), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public ICollection<ProfileRank>? ProfileRanks { get; set; }
    }
}