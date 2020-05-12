using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class ProfileRank : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        public ProfileFull? Profile { get; set; }

        [Display(Name = nameof(RankId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        public Guid RankId { get; set; } = default!;

        [Display(Name = nameof(Rank), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        public Rank? Rank { get; set; }
    }
}