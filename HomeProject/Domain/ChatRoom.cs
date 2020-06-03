using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ChatRoom : DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)] public string ChatRoomTitle { get; set; } = default!;

        [MaxLength(300)] public string? ChatRoomImageUrl { get; set; }
        public Guid? ChatRoomImageId { get; set; }

        public ICollection<ChatMember>? ChatMembers { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}