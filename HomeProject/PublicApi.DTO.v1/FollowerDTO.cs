using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class FollowerProfileDTO
    {
        public string UserName { get; set; } = default!;
        public Guid? ProfileAvatarId { get; set; }
    }
    
    public class FollowerAdminDTO: DomainEntityBaseMetadata
    {
        //who want to follow
        public Guid FollowerProfileId { get; set; } = default!;

        //who will have new follower
        public Guid ProfileId { get; set; } = default!;
    }
}