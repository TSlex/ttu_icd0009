using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using Domain.Translation;

namespace Domain
{
    public class Gift: DomainEntityBaseMetadata
    {
//        [MaxLength(100)] public string GiftName { get; set; } = default!;

        public Guid GiftNameId { get; set; } = default!;
        public LangString? GiftName { get; set; } = default!;

        [MaxLength(100)] public string GiftCode { get; set; } = default!;
        
        [MaxLength(300)] public string? GiftImageUrl { get; set; }
        
        public Guid? GiftImageId { get; set; }
        public Image? GiftImage { get; set; }

        public int Price { get; set; }
        
        public ICollection<ProfileGift>? ProfileGifts { get; set; }
    }
}