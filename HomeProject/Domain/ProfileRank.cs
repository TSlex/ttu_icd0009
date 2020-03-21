using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ProfileRank: DomainEntity
    {
        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        [MaxLength(36)] public string RankId { get; set; } = default!;
        public Rank? Rank { get; set; }
    }
}