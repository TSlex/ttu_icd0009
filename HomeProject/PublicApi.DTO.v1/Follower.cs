using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class Follower: DomainEntity
    {
        [MaxLength(36)] public string ProfileId { get; set; } = default!;

        [MaxLength(36)] public string FollowerProfileId { get; set; } = default!;
    }
}