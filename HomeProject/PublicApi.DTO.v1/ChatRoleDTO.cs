using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatRoleDTO
    {
        public string RoleTitle { get; set; } = default!;
        public string RoleTitleValue { get; set; } = default!;
        
        public bool CanRenameRoom { get; set; }
        public bool CanEditMembers { get; set; }
        public bool CanWriteMessages { get; set; }
        public bool CanEditAllMessages { get; set; }
        public bool CanEditMessages { get; set; }
    }
    
    public class ChatRoleAdminDTO: DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(RoleTitle), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string RoleTitle { get; set; } = default!;

        [Display(Name = nameof(RoleTitleValueId), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid RoleTitleValueId { get; set; } = default!;

        [Display(Name = nameof(RoleTitleValue), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? RoleTitleValue { get; set; } = default!;

        [Display(Name = nameof(CanRenameRoom), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public bool CanRenameRoom { get; set; }

        [Display(Name = nameof(CanEditMembers), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public bool CanEditMembers { get; set; }

        [Display(Name = nameof(CanWriteMessages), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public bool CanWriteMessages { get; set; }

        [Display(Name = nameof(CanEditAllMessages), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public bool CanEditAllMessages { get; set; }

        [Display(Name = nameof(CanEditMessages), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public bool CanEditMessages { get; set; }
    }
}