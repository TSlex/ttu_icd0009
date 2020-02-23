using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ChatRoom: DomainEntityMetadata
    {
        [MaxLength(100)] public string ChatRoomTitle { get; set; } = default!;
        [MaxLength(100)] public string? LastMessageValue { get; set; }

        public DateTime? LastMessageDateTime { get; set; }

        public ICollection<ChatMember>? ChatMembers { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}