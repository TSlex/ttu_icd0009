using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ChatRole: DomainEntity
    {
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;
        
        public ICollection<ChatMember>? ChatMembers { get; set; }
    }
}