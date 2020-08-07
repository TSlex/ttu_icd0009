using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;
using DomainEntityBaseMetaSoftDelete = BLL.App.DTO.Base.DomainEntityBaseMetaSoftDelete;

namespace BLL.App.DTO
{
    public class ProfileGift : DomainEntityBaseMetaSoftDelete
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(GiftId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid GiftId { get; set; } = default!;

        [Display(Name = nameof(Gift), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Gift? Gift { get; set; }

        [Display(Name = nameof(GiftDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public DateTime GiftDateTime { get; set; } = DateTime.UtcNow;

        [Display(Name = nameof(Price), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int Price { get; set; }

        [Display(Name = nameof(FromProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Guid? FromProfileId { get; set; } = null;

        [Display(Name = nameof(FromProfile), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Profile? FromProfile { get; set; }

        [Display(Name = nameof(Message), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? Message { get; set; }

        [Display(Name = nameof(ReturnUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? ReturnUrl { get; set; }
    }

    public class ProfileGiftCreate : ProfileGift
    {
        [Display(Name = nameof(GiftGallery), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public IEnumerable<Gift> GiftGallery { get; set; } = default!;
    }
}