using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class FollowerDTO: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid FollowerProfileId { get; set; } = default!;
    }
    
    public class FollowerProfileDTO
    {
        public string UserName { get; set; } = default!;
//        public string? ProfileAvatarUrl { get; set; }
        public Guid? ProfileAvatarId { get; set; }
    }
}