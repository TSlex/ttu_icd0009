using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DomainEntityBaseMetaSoftUpdateDelete = BLL.App.DTO.Base.DomainEntityBaseMetaSoftUpdateDelete;

namespace BLL.App.DTO
{
    public class Image : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(ImageUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ImageUrl { get; set; }

        [Display(Name = nameof(OriginalImageUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? OriginalImageUrl { get; set; }

        [Display(Name = nameof(HeightPx), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [Range(0, 10000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int HeightPx { get; set; }

        [Display(Name = nameof(WidthPx), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [Range(0, 10000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int WidthPx { get; set; }

        [Display(Name = nameof(PaddingTop), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        public int PaddingTop { get; set; }

        [Display(Name = nameof(PaddingRight), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        public int PaddingRight { get; set; }

        [Display(Name = nameof(PaddingBottom), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        public int PaddingBottom { get; set; }

        [Display(Name = nameof(PaddingLeft), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        public int PaddingLeft { get; set; }

        [Display(Name = nameof(ImageFile), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = nameof(ImageType), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public ImageType ImageType { get; set; } = default!;

        [Display(Name = nameof(ImageFor), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        public Guid? ImageFor { get; set; }

        [JsonIgnore]
        [Display(Name = nameof(Profiles), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        public ICollection<Domain.Profile>? Profiles { get; set; }
        
        [JsonIgnore]
        [Display(Name = nameof(Posts), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        public ICollection<Post>? Posts { get; set; }
        
        [JsonIgnore]
        [Display(Name = nameof(Gifts), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        public ICollection<Gift>? Gifts { get; set; }
    }
}