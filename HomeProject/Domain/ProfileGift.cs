using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ProfileGift: DomainEntityBaseMetaSoftDelete
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid GiftId { get; set; } = default!;
        public Gift? Gift { get; set; }
        
        public DateTime GiftDateTime { get; set; } = DateTime.Now;
        
        //what price was
        public int Price { get; set; }
    }
}