using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatMemberDTO
    {
        public Guid Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string? ProfileAvatarUrl { get; set; }
        public string ChatRole { get; set; } = default!;
    }
}