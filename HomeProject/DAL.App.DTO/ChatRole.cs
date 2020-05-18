using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace DAL.App.DTO
{
    public class ChatRole: DomainEntityBaseMetaSoftUpdateDelete
    {
        public Guid RoleTitleValueId { get; set; } = default!;
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;
        
        public ICollection<ChatMember>? ChatMembers { get; set; }
    }
}