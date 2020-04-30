using System;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Follower: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public ProfileFull? Profile { get; set; }
        
        public Guid FollowerProfileId { get; set; } = default!;
        public ProfileFull? FollowerProfile { get; set; }
    }
}