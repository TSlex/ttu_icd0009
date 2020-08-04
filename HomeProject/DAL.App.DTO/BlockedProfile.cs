using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class BlockedProfile: DomainEntityBaseMetadata
    {
        // Profile who wants to block BProfile
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        // BProfile blocked by Profile
        public Guid BProfileId { get; set; } = default!;
        public Profile? BProfile { get; set; }

        [MaxLength(200)] public string? Reason { get; set; } //filed by enum
    }
}