using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace PublicApi.DTO.v1
{
    public class BlockedProfileDTO
    {
        public string UserName { get; set; } = default!;
        public Guid? ProfileAvatarId { get; set; }
    }
    
    public class BlockedProfileAdminDTO: DomainEntityBaseMetadata
    {
        // Profile who wants to block BProfile
        [Display(Name = nameof(ProfileId),
            ResourceType = typeof(Resourses.BLL.App.DTO.BlockedProfiles.BlockedProfiles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        // BProfile blocked by Profile
        [Display(Name = nameof(BProfileId),
            ResourceType = typeof(Resourses.BLL.App.DTO.BlockedProfiles.BlockedProfiles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid BProfileId { get; set; } = default!;
    }
}