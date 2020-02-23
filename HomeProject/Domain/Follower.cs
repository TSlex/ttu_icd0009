using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Follower: DomainEntityMetadata
    {
        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        [MaxLength(36)] public string FollowerProfileId { get; set; } = default!;
        public Profile? FollowerProfile { get; set; }
    }
}