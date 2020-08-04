using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;
using DomainEntityBaseMetaSoftUpdateDelete = BLL.App.DTO.Base.DomainEntityBaseMetaSoftUpdateDelete;

namespace BLL.App.DTO
{
    public class Gift : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(GiftNameId), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid GiftNameId { get; set; } = default!;
        
        [Display(Name = nameof(GiftName), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string GiftName { get; set; } = default!;

        [Display(Name = nameof(GiftCode), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string GiftCode { get; set; } = default!;

        [Display(Name = nameof(GiftImageId), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public Guid? GiftImageId { get; set; }

        [Display(Name = nameof(GiftImage), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public Image? GiftImage { get; set; }

        [Display(Name = nameof(Price), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int Price { get; set; }

        [Display(Name = nameof(ProfileGifts), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public ICollection<ProfileGift>? ProfileGifts { get; set; }
    }

    public class GiftsCount
    {
        [Display(Name = nameof(Count), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        public int Count { get; set; }
    }
}