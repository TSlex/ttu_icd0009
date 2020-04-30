using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class BlockedProfile: DomainEntityBaseMetadata
    {
        // Profile who wants to block BProfile
        public Guid ProfileId { get; set; } = default!;
        public ProfileFull? Profile { get; set; }

        // BProfile blocked by Profile
        public Guid BProfileId { get; set; } = default!;
        public ProfileFull? BProfile { get; set; }

        [MaxLength(200)] public string? Reason { get; set; } //filed by enum
    }
}