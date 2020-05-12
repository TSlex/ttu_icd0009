﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Gift : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(GiftName), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [MaxLength(100)]
        public string GiftName { get; set; } = default!;

        [Display(Name = nameof(GiftCode), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [MaxLength(100)]
        public string GiftCode { get; set; } = default!;

        [Display(Name = nameof(GiftImageUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [MaxLength(300)]
        public string? GiftImageUrl { get; set; }

        [Display(Name = nameof(GiftImageId), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public Guid? GiftImageId { get; set; }

        [Display(Name = nameof(GiftImage), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public Image? GiftImage { get; set; }

        [Display(Name = nameof(Price), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public int Price { get; set; }

        [Display(Name = nameof(GiftDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public DateTime GiftDateTime { get; set; }

        [Display(Name = nameof(ProfileGifts), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public ICollection<ProfileGift>? ProfileGifts { get; set; }
    }

    public class GiftsCount
    {
        [Display(Name = nameof(Count), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public int Count { get; set; }
    }
}