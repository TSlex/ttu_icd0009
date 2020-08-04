using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;
using DomainEntityBaseMetadata = BLL.App.DTO.Base.DomainEntityBaseMetadata;

namespace BLL.App.DTO
{
    public class Follower : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(FollowerProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid FollowerProfileId { get; set; } = default!;

        [Display(Name = nameof(FollowerProfile), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        public Profile? FollowerProfile { get; set; }
    }
}