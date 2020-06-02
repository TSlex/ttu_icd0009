using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class LoginDTO
    {
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(Email),
            ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [EmailAddress(ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
            ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; } = default!;
        
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(Password),
            ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}