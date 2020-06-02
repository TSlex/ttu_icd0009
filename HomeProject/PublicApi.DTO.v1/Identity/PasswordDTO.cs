using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class PasswordDTO
    {
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Resourses.Views.Identity.Identity))]
        public string CurrentPassword { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resourses.Views.Identity.Identity))]
        public string NewPassword { get; set; } = default!;
    }
}