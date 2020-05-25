using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using DAL.Base;
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
    
    public class ImageAdminDTO: DomainEntityBaseMetaSoftUpdateDelete
    {
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
}