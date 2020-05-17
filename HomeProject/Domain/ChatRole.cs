using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using Domain.Translation;

namespace Domain
{
    public class ChatRole: DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;
        
        public Guid RoleTitleValueId { get; set; } = default!;
        public LangString? RoleTitleValue { get; set; } = default!;
        
        public ICollection<ChatMember>? ChatMembers { get; set; }
    }
}