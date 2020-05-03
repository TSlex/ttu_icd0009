using System.ComponentModel.DataAnnotations;
using DAL.Base;
using Microsoft.AspNetCore.Http;

namespace BLL.App.DTO
{
    public class Image: DomainEntityBaseMetadata
    {
        [MaxLength(300)] public string? ImageUrl { get; set; }
        [Range(0, 10000)] public int HeightPx { get; set; }
        [Range(0, 10000)] public int WidthPx { get; set; }
        
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
        
        public IFormFile? ImageFile { get; set; }
    }
}