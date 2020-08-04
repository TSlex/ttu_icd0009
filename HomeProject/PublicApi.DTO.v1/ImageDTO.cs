using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ee.itcollege.aleksi.DAL.Base;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace PublicApi.DTO.v1
{
    public class ImageDTO
    {
        public Guid? Id { get; set; }

        public string? ImageUrl { get; set; }
        public string? OriginalImageUrl { get; set; }

        [Range(0, int.MaxValue)] public int HeightPx { get; set; }
        [Range(0, int.MaxValue)] public int WidthPx { get; set; }

        [Range(0, int.MaxValue)] public int PaddingTop { get; set; }
        [Range(0, int.MaxValue)] public int PaddingRight { get; set; }
        [Range(0, int.MaxValue)] public int PaddingBottom { get; set; }
        [Range(0, int.MaxValue)] public int PaddingLeft { get; set; }

        public IFormFile? ImageFile { get; set; }

        public ImageType ImageType { get; set; }
        
        public Guid? ImageFor { get; set; }
    }
    
    public class ImagePostDTO
    {
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
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public IFormFile ImageFile { get; set; } = default!;

        [Display(Name = nameof(ImageType), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public ImageType ImageType { get; set; } = default!;

        [Display(Name = nameof(ImageFor), ResourceType = typeof(Resourses.BLL.App.DTO.Images.Images))]
        public Guid? ImageFor { get; set; }
    }
    
    public class ImagePutDTO
    {
        [Display(Name = nameof(Id), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid Id { get; set; }
        
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
    }
    
    public class ImageAdminDTO: DomainEntityBaseMetaSoftUpdateDelete
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
    }
}