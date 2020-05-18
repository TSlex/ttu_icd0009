﻿using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace DAL.App.DTO
{
    public class ChatMember: DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)] public string? ChatRoomTitle { get; set; }

        public Guid ChatRoomId { get; set; } = default!;
        public ChatRoom? ChatRoom { get; set; }

        public Guid ChatRoleId { get; set; } = default!;
        public ChatRole? ChatRole { get; set; }
        
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
    }
}