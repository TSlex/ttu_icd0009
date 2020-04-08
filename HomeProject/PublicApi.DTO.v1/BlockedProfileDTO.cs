

using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class BlockedProfileDTO: DomainEntity
    {
        // Profile who wants to block BProfile
        public Guid ProfileId { get; set; } = default!;

        // BProfile blocked by Profile
        public Guid BProfileId { get; set; } = default!;

        [MaxLength(200)] public string? Reason { get; set; } //filed by enum
    }
}