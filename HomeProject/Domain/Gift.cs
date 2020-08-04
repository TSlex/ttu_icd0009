using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;
using Domain.Translation;

namespace Domain
{
    public class Gift : DomainEntityBaseMetaSoftUpdateDelete
    {
        public Guid GiftNameId { get; set; } = default!;
        public LangString? GiftName { get; set; } = default!;

        [MaxLength(100)] public string GiftCode { get; set; } = default!;

        public Guid? GiftImageId { get; set; }
        public Image? GiftImage { get; set; }

        public int Price { get; set; }

        public ICollection<ProfileGift>? ProfileGifts { get; set; }
    }
}