using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class ChatRole: DomainEntityBaseMetadata
    {    
        [Display(Name = nameof(RoleTitle), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;
        
        [Display(Name = nameof(ChatMembers), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRoles.ChatRoles))]
        public ICollection<ChatMember>? ChatMembers { get; set; }
    }
}