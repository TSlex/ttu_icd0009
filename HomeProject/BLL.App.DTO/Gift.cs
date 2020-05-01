using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Gift: DomainEntityBaseMetadata
    {
        [MaxLength(100)] public string GiftName { get; set; } = default!;
        [MaxLength(100)] public string GiftCode { get; set; } = default!;
        [MaxLength(300)] public string? GiftImageUrl { get; set; }
        
        public int Price { get; set; }
        
        public ICollection<ProfileGift>? ProfileGifts { get; set; }
    }
}