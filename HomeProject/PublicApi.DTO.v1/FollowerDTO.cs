using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class FollowerProfileDTO
    {
        public string UserName { get; set; } = default!;
        public Guid? ProfileAvatarId { get; set; }
    }
    
    public class FollowerAdminDTO: DomainEntityBaseMetadata
    {
        //who will have new follower
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        //who want to follow
        [Display(Name = nameof(FollowerProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid FollowerProfileId { get; set; } = default!;
    }
}