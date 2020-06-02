using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ProfileGiftDTO
    {
        public Guid Id { get; set; } = default!;
        
        public string GiftName { get; set; } = default!;
        
        public string Username { get; set; } = default!;
        public string? FromUsername { get; set; }

        public Guid? ImageId { get; set; } = default!;

        public DateTime GiftDateTime { get; set; } = default!;

        public int Price { get; set; }

        public string? Message { get; set; }
    }

    public class ProfileGiftCreateDTO
    {
        [Display(Name = "UserName", ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string Username { get; set; } = default!;
        
        [Display(Name = "FromProfile", ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public string? FromUsername { get; set; }

        [Display(Name = nameof(GiftCode), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string GiftCode { get; set; } = default!;
        
        [Display(Name = nameof(Message), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? Message { get; set; }
    }

    public class ProfileGiftAdminDTO : DomainEntityBaseMetaSoftDelete
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;
        
        [Display(Name = nameof(GiftId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid GiftId { get; set; } = default!;
        
        [Display(Name = nameof(GiftDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public DateTime GiftDateTime { get; set; } = default!;
        
        [Display(Name = nameof(Price), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int Price { get; set; }
        
        [Display(Name = nameof(FromProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        public Guid? FromProfileId { get; set; }
        
        [Display(Name = nameof(Message), ResourceType = typeof(Resourses.BLL.App.DTO.ProfileGifts.ProfileGifts))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? Message { get; set; }
    }
}