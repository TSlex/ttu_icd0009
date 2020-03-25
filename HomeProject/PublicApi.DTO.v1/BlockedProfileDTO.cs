

using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class BlockedProfileDTO: DomainEntity
    {
        // Profile who wants to block BProfile
        [MaxLength(36)] public string ProfileId { get; set; } = default!;

        // BProfile blocked by Profile
        [MaxLength(36)] public string BProfileId { get; set; } = default!;

        [MaxLength(200)] public string? Reason { get; set; } //filed by enum
    }
}