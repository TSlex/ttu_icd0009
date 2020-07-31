using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Identity;

namespace PublicApi.DTO.v1
{
    public class ProfileDTO
    {
        public DateTime? LastLoginDateTime { get; set; }

        public string? UserName { get; set; }
        public string? ProfileFullName { get; set; }

        public string? ProfileWorkPlace { get; set; }

        public string? ProfileStatus { get; set; }
        public string? ProfileAbout { get; set; }

        public Guid? ProfileAvatarId { get; set; }

        public ProfileGender ProfileGender { get; set; }
        public string? ProfileGenderOwn { get; set; }

        public int FollowersCount { get; set; } = 0;
        public int FollowedCount { get; set; } = 0;
        public int PostsCount { get; set; } = 0;

        public int Experience { get; set; } = 0;

        public bool IsUserBlocked { get; set; }
        public bool IsUserFollows { get; set; }
        public bool IsUserBlocks { get; set; }
    }

    public class ProfileAdminDTO : MUser
    {
        [Display(Name = nameof(LastLoginDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public DateTime? LastLoginDateTime { get; set; }

        [Display(Name = nameof(ProfileFullName), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MinLength(1, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MinLength")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileFullName { get; set; }

        [Display(Name = nameof(ProfileWorkPlace), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileWorkPlace { get; set; }

        [Display(Name = nameof(ProfileStatus), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileStatus { get; set; }

        [Display(Name = nameof(ProfileAvatarId), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public Guid? ProfileAvatarId { get; set; }

        [Display(Name = nameof(ProfileAbout), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileAbout { get; set; }

        [Display(Name = nameof(ProfileGender), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;

        [Display(Name = nameof(ProfileGenderOwn), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileGenderOwn { get; set; }

        public int FollowersCount { get; set; }
        public int FollowedCount { get; set; }
        public int PostsCount { get; set; }

        [Display(Name = nameof(Experience), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int Experience { get; set; }
        
        [Display(Name = nameof(Password),
            ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}