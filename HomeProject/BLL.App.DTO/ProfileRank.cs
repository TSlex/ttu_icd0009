using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using DomainEntityBaseMetaSoftDelete = BLL.App.DTO.Base.DomainEntityBaseMetaSoftDelete;

namespace BLL.App.DTO
{
    public class ProfileRank : DomainEntityBaseMetaSoftDelete
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(RankId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public Guid RankId { get; set; } = default!;

        [Display(Name = nameof(Rank), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        public Rank? Rank { get; set; }
    }
}