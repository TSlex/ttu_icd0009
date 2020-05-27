using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatMemberDTO
    {
        public Guid Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public Guid? ProfileAvatarId { get; set; }
        public string ChatRole { get; set; } = default!;
        public string ChatRoleValue { get; set; } = default!;

        public bool CanRenameRoom { get; set; }
        public bool CanEditMembers { get; set; }
        public bool CanWriteMessages { get; set; }
        public bool CanEditAllMessages { get; set; }
        public bool CanEditMessages { get; set; }
    }

    public class ChatMemberAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)] public string? ChatRoomTitle { get; set; }

        public Guid ChatRoomId { get; set; } = default!;

        public Guid ChatRoleId { get; set; } = default!;

        public Guid ProfileId { get; set; } = default!;
    }
}