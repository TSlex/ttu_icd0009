using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class ProfileGift : DomainEntityBaseMetaSoftDelete
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(GiftId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Guid GiftId { get; set; } = default!;

        [Display(Name = nameof(Gift), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Gift? Gift { get; set; }

        [Display(Name = nameof(GiftDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public DateTime GiftDateTime { get; set; } = DateTime.Now;

        //what price was
        [Display(Name = nameof(Price), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public int Price { get; set; }

        [Display(Name = nameof(ReturnUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? ReturnUrl { get; set; }
    }

    public class ProfileGiftCreate : ProfileGift
    {
        [Display(Name = nameof(GiftGallery), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public IEnumerable<Gift> GiftGallery { get; set; } = default!;
    }
}