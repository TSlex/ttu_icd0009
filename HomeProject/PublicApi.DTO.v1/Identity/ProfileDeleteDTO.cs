using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class ProfileDeleteDTO
    {
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(Password),
            ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}