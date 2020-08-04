using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ProfileRankAdminDTO: DomainEntityBaseMetaSoftDelete
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;
        
        [Display(Name = nameof(RankId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileRanks.ProfileRanks))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid RankId { get; set; } = default!;
    }
}