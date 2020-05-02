using System;
using System.Collections.Generic;
using DAL.Base;

namespace BLL.App.DTO
{
    public class ProfileGift: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public ProfileFull? Profile { get; set; }
        
        public Guid GiftId { get; set; } = default!;
        public Gift? Gift { get; set; }
        
        public DateTime GiftDateTime { get; set; } = DateTime.Now;
        
        //what price was
        public int Price { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class ProfileGiftCreate : ProfileGift
    {
        public IEnumerable<Gift> GiftGallery { get; set; } = default!;
    }
}