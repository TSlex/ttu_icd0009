using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class BlockedProfileDTO
    {
        public string UserName { get; set; } = default!;
        public string? ProfileAvatarUrl { get; set; }
    }
}