using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using Domain.Enums;

namespace Domain
{
    public class Image: DomainEntityBaseMetadata
    {
        [MaxLength(300)] public string ImageUrl { get; set; }
        [MaxLength(300)] public string OriginalImageUrl { get; set; }
        
        [Range(0, int.MaxValue)] public ImageType ImageType { get; set; } = ImageType.Undefined;
        public Guid ImageFor { get; set; } = default!;
        
        [Range(0, 10000)] public int HeightPx { get; set; }
        [Range(0, 10000)] public int WidthPx { get; set; }
        
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
    }
}