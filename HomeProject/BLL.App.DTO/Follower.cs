using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Follower : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(FollowerProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        public Guid FollowerProfileId { get; set; } = default!;

        [Display(Name = nameof(FollowerProfile), ResourceType = typeof(Resourses.BLL.App.DTO.Followers.Followers))]
        public Profile? FollowerProfile { get; set; }
    }
}