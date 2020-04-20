using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatMemberDTO: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid ChatRoleId { get; set; } = default!;

        public Guid ChatRoomId { get; set; } = default!;
    }
}