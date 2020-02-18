namespace Domain
{
    public class ProfileGift
    {
        public string ProfileGiftId { get; set; }
        
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string GiftId { get; set; }
        public Gift Gift { get; set; }
    }
}