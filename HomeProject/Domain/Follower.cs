using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Follower : DomainEntityBaseMetadata
    {
        //who want to follow
        public Guid FollowerProfileId { get; set; } = default!;
        public Profile? FollowerProfile { get; set; }

        //who will have new follower
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
    }
}