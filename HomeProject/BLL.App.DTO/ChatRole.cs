using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class ChatRole: DomainEntityBaseMetaSoftUpdateDelete
    {    
        [Display(Name = nameof(RoleTitleValueId), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public Guid RoleTitleValueId { get; set; } = default!;
        
        [Display(Name = nameof(RoleTitleValue), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public string? RoleTitleValue { get; set; } = default!;
        
        [Display(Name = nameof(RoleTitle), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;
        
        [Display(Name = nameof(ChatMembers), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public ICollection<ChatMember>? ChatMembers { get; set; }
        
        [Display(Name = nameof(CanRenameRoom), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public bool CanRenameRoom { get; set; }
        
        [Display(Name = nameof(CanEditMembers), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public bool CanEditMembers { get; set; }
        
        [Display(Name = nameof(CanWriteMessages), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public bool CanWriteMessages { get; set; }
        
        [Display(Name = nameof(CanEditAllMessages), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public bool CanEditAllMessages { get; set; }
        
        [Display(Name = nameof(CanEditMessages), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public bool CanEditMessages { get; set; }
    }
}