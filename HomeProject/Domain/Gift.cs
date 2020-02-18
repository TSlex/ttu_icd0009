using System.Collections.Generic;

namespace Domain
{
    public class Gift
    {
        public string GiftId { get; set; }
        public string GiftName { get; set; }
        public string GiftImageUrl { get; set; }
        
        public ICollection<ProfileGift> ProfileGifts { get; set; }
    }
}