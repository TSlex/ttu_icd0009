using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ProfileGift: DomainEntity
    {
        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        [MaxLength(36)] public string GiftId { get; set; } = default!;
        public Gift? Gift { get; set; }
    }
}