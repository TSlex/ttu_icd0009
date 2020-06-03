using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using Domain.Enums;

namespace Domain
{
    public class Image : DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(300)] public string ImageUrl { get; set; } = default!;
        [MaxLength(300)] public string OriginalImageUrl { get; set; } = default!;

        [Range(0, int.MaxValue)] public ImageType ImageType { get; set; } = ImageType.Undefined;
        public Guid ImageFor { get; set; } = default!;

        [Range(0, 10000)] public int HeightPx { get; set; }
        [Range(0, 10000)] public int WidthPx { get; set; }

        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }

        public ICollection<Profile>? Profiles { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Gift>? Gifts { get; set; }
    }
}