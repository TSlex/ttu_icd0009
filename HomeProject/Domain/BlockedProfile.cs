using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Base;

namespace Domain
{
    public class BlockedProfile: DomainEntityMetadata
    {
        // Profile who wants to block BProfile
        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        // BProfile blocked by Profile
        [MaxLength(36)] public string BProfileId { get; set; } = default!;
        public Profile? BProfile { get; set; }

        [MaxLength(200)] public string? Reason { get; set; } //filed by enum
    }
}