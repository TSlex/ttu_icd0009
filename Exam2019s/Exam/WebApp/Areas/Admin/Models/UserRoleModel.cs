using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRoleModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Domain.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(UserId),
            ResourceType = typeof(Resources.Domain.Common))]
        public Guid UserId { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Domain.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "UserRoleId",
            ResourceType = typeof(Resources.Domain.Common))]
        public Guid OldRoleId { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Domain.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "UserRoleId",
            ResourceType = typeof(Resources.Domain.Common))]
        public Guid NewRoleId { get; set; } = default!;
    }
}