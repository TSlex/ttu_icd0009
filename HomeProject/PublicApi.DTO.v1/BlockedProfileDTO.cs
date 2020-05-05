using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class BlockedProfileDTO
    {
        public string UserName { get; set; }
        public string? ProfileAvatarUrl { get; set; }
    }
}