using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class Gift: DomainEntityBaseMetaSoftUpdateDelete
    {
        public Guid GiftNameId { get; set; } = default!;
        public string GiftName { get; set; } = default!;
        public string GiftCode { get; set; } = default!;

        public int Price { get; set; }
        
        public Guid? GiftImageId { get; set; }
        public Image? GiftImage { get; set; }

        public ICollection<ProfileGift>? ProfileGifts { get; set; }
    }
}