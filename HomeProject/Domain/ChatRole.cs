using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;
using Domain.Translation;

namespace Domain
{
    public class ChatRole : DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;

        public Guid RoleTitleValueId { get; set; } = default!;
        public LangString? RoleTitleValue { get; set; } = default!;

        public bool CanRenameRoom { get; set; }
        public bool CanEditMembers { get; set; }
        public bool CanWriteMessages { get; set; }
        public bool CanEditAllMessages { get; set; }
        public bool CanEditMessages { get; set; }

        public ICollection<ChatMember>? ChatMembers { get; set; }
    }
}