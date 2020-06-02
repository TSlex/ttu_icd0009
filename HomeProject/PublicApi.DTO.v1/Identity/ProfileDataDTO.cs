using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace PublicApi.DTO.v1.Identity
{
    public class ProfileDataDTO
    {
        [Display(Name = "UserName", ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string Username { set; get; }

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

        [Display(Name = nameof(ProfileAbout), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileAbout { get; set; }

        [Phone(ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
            ErrorMessageResourceName = "InvalidPhone")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Display(Name = nameof(PhoneNumber), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string? PhoneNumber { get; set; }

        [Display(Name = nameof(ProfileGender), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;

        [Display(Name = nameof(ProfileGenderOwn), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileGenderOwn { get; set; }
    }
}