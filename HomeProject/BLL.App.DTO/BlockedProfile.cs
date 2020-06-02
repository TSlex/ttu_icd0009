using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using DomainEntityBaseMetadata = BLL.App.DTO.Base.DomainEntityBaseMetadata;

namespace BLL.App.DTO
{
    public class BlockedProfile : DomainEntityBaseMetadata
    {
        // Profile who wants to block BProfile
        [Display(Name = nameof(ProfileId),
            ResourceType = typeof(Resourses.BLL.App.DTO.BlockedProfiles.BlockedProfiles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile),
            ResourceType = typeof(Resourses.BLL.App.DTO.BlockedProfiles.BlockedProfiles))]
        public Profile? Profile { get; set; }

        // BProfile blocked by Profile
        [Display(Name = nameof(BProfileId),
            ResourceType = typeof(Resourses.BLL.App.DTO.BlockedProfiles.BlockedProfiles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid BProfileId { get; set; } = default!;

        [Display(Name = nameof(BProfile),
            ResourceType = typeof(Resourses.BLL.App.DTO.BlockedProfiles.BlockedProfiles))]
        public Profile? BProfile { get; set; }

        [Display(Name = nameof(Reason),
            ResourceType = typeof(Resourses.BLL.App.DTO.BlockedProfiles.BlockedProfiles))]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? Reason { get; set; } //filed by enum
    }
}