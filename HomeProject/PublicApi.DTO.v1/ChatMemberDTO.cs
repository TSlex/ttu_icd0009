using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatMemberDTO: DomainEntity
    {
        [MaxLength(36)] public string ProfileId { get; set; } = default!;

        [MaxLength(36)] public string ChatRoleId { get; set; } = default!;

        [MaxLength(36)] public string ChatRoomId { get; set; } = default!;
    }
}