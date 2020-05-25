using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class BlockedProfileDTO
    {
        public string UserName { get; set; } = default!;
        public Guid? ProfileAvatarId { get; set; }
    }
    
    public class BlockedProfileAdminDTO: DomainEntityBaseMetadata
    {
        // Profile who wants to block BProfile
        public Guid ProfileId { get; set; } = default!;

        // BProfile blocked by Profile
        public Guid BProfileId { get; set; } = default!;
    }
}