using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.App.DTO.Identity
{
    public class UserRoleModel
    {
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(UserId),
            ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public Guid UserId { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "UserRoleId",
            ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public Guid OldRoleId { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "UserRoleId",
            ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public Guid NewRoleId { get; set; } = default!;
    }
}