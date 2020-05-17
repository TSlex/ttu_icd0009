using System;
using System.Collections.Generic;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace PublicApi.DTO.v1
{
    public class ImageDTO
    {
        public Guid? Id { get; set; }
        
        public string? ImageUrl { get; set; }
        public string? OriginalImageUrl { get; set; }

        
        public int HeightPx { get; set; }
        public int WidthPx { get; set; }

        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }

        public IFormFile? ImageFile { get; set; }
        
        public ImageType? ImageType { get; set; }
        public Guid? ImageFor { get; set; }
    }
}