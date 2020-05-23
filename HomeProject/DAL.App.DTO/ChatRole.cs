using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace DAL.App.DTO
{
    public class ChatRole: DomainEntityBaseMetaSoftUpdateDelete
    {
        public Guid RoleTitleValueId { get; set; } = default!;
        public string? RoleTitleValue { get; set; } = default!;
        
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;
        
        public bool CanRenameRoom { get; set; }
        public bool CanEditMembers { get; set; }
        public bool CanWriteMessages { get; set; }
        public bool CanEditAllMessages { get; set; }
        public bool CanEditMessages { get; set; }
        
        public ICollection<ChatMember>? ChatMembers { get; set; }
    }
}