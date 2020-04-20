using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ChatMember: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        public Guid ChatRoleId { get; set; } = default!;
        public ChatRole? ChatRole { get; set; }

        public Guid ChatRoomId { get; set; } = default!;
        public ChatRoom? ChatRoom { get; set; }
    }
}