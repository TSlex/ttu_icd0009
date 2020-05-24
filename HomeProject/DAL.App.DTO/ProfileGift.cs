using System;
using DAL.Base;

namespace DAL.App.DTO
{
    public class ProfileGift: DomainEntityBaseMetaSoftDelete
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid GiftId { get; set; } = default!;
        public Gift? Gift { get; set; }
        
        public DateTime GiftDateTime { get; set; } = DateTime.Now;
        public int Price { get; set; }
        
        public Guid? FromProfileId { get; set; }
        public Profile? FromProfile { get; set; }

        public string? Message { get; set; }
    }
}