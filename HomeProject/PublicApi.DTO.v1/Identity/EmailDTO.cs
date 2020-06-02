using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class EmailDTO
    {
        [EmailAddress(ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
            ErrorMessageResourceName = "InvalidEmail")]
        [Display(Name = "CurrentEmail", ResourceType = typeof(Resourses.Views.Identity.Identity))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string CurrentEmail { get; set; } = default!;

        [EmailAddress(ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
            ErrorMessageResourceName = "InvalidEmail")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "Email",
            ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string NewEmail { get; set; } = default!;
    }
}