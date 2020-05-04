using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class PostDTO: DomainEntityBaseMetadata
    {
        [MaxLength(100)] public string PostTitle { get; set; } = default!;
        [MaxLength(300)] public string? PostImageUrl { get; set; }
        [MaxLength(500)] public string? PostDescription { get; set; }

        public DateTime PostPublicationDateTime { get; set; } = DateTime.Now;

        public int PostFavoritesCount { get; set; } = 0;
        public int PostCommentsCount { get; set; } = 0;

        public Guid ProfileId { get; set; } = default!;
    }

    public class PostCreateDTO
    {
        public Guid Id { get; set; } = default!;
        public Guid ProfileId { get; set; } = default!;
        
        public string PostTitle { get; set; } = default!;
        public string? PostDescription { get; set; }

        public Guid? PostImageId { get; set; }
        public string? PostImageUrl { get; set; }
    }

    public class PostGetDTO
    {
        public Guid Id { get; set; } = default!;
        public string PostTitle { get; set; } = default!;
        public string? PostDescription { get; set; }
        
        public Guid? PostImageId { get; set; }
        public string? PostImageUrl { get; set; }

        public DateTime PostPublicationDateTime { get; set; } = default!;
        
        public int PostFavoritesCount { get; set; }
        public int PostCommentsCount { get; set; }
        
        public bool IsFavorite { get; set; }
    }

    public class PostEditDTO
    {
        public Guid Id { get; set; } = default!;
        public string PostTitle { get; set; } = default!;
        public string? PostDescription { get; set; }
    }
}