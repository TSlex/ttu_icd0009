using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Follower: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid FollowerProfileId { get; set; } = default!;
        public Profile? FollowerProfile { get; set; }
    }
}