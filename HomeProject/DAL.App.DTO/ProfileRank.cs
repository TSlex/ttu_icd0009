using System;
using DAL.Base;

namespace DAL.App.DTO
{
    public class ProfileRank: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid RankId { get; set; } = default!;
        public Rank? Rank { get; set; }
    }
}