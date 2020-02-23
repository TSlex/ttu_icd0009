using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ChatMember: DomainEntityMetadata
    {
        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        [MaxLength(36)] public string ChatRoleId { get; set; } = default!;
        public ChatRole? ChatRole { get; set; }

        [MaxLength(36)] public string ChatRoomId { get; set; } = default!;
        public ChatRoom? ChatRoom { get; set; }
    }
}