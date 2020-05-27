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
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;
        
        public Guid RoleTitleValueId { get; set; } = default!;

        public bool CanRenameRoom { get; set; }
        public bool CanEditMembers { get; set; }
        public bool CanWriteMessages { get; set; }
        public bool CanEditAllMessages { get; set; }
        public bool CanEditMessages { get; set; }
    }
}