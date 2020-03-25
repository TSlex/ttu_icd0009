﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatRole: DomainEntity
    {
        [MaxLength(200)] public string RoleTitle { get; set; } = default!;
        
        public ICollection<ChatMemberDTO>? ChatMembers { get; set; }
    }
}