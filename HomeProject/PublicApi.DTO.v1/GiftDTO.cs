using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    /// <summary>
    /// Get only
    /// </summary>
    public class GiftDTO
    {
        public string GiftName { get; set; } = default!;
        public string GiftCode { get; set; } = default!;

        public Guid? GiftImageId { get; set; }

        public int Price { get; set; }
    }

    /// <summary>
    /// Get only
    /// </summary>
    public class GiftsCountDTO
    {
        public int Count { get; set; }
    }
    
    public class GiftAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
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

        [Display(Name = nameof(Price), ResourceType = typeof(Resourses.BLL.App.DTO.Gifts.Gifts))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int Price { get; set; }
    }
}