using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace DAL.App.DTO
{
    public class Gift: DomainEntityBaseMetadata
    {
        public string GiftName { get; set; } = default!;
        public string GiftCode { get; set; } = default!;
        public string? GiftImageUrl { get; set; }
        
        public int Price { get; set; }
        
        public ICollection<ProfileGift>? ProfileGifts { get; set; }
    }
}