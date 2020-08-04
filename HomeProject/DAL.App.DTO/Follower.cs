using System;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class Follower: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid FollowerProfileId { get; set; } = default!;
        public Profile? FollowerProfile { get; set; }
    }
}