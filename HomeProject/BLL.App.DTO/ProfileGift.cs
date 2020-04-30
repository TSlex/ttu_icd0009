using System;
using DAL.Base;

namespace BLL.App.DTO
{
    public class ProfileGift: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public ProfileFull? Profile { get; set; }
        
        public Guid GiftId { get; set; } = default!;
        public Gift? Gift { get; set; }
    }
}