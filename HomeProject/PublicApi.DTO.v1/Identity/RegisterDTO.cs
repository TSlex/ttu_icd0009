using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class RegisterDTO
    {
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "UserName",
            ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
            ErrorMessageResourceName = "InvalidEmail")]
        [Display(Name = nameof(Email),
            ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string Email { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Password),
            ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string Password { get; set; } = default!;
    }
}