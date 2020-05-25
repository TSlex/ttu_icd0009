using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class PostCreateDTO
    {
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
        
        public string ProfileUsername { get; set; } = default!;
        
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
    
    public class PostAdminDTO: DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)] public string PostTitle { get; set; } = default!;

        public Guid? PostImageId { get; set; }

        [MaxLength(500)] public string? PostDescription { get; set; }

        public DateTime PostPublicationDateTime { get; set; } = default!;
        
        public int PostFavoritesCount { get; set; }
        public int PostCommentsCount { get; set; }
        
        public Guid ProfileId { get; set; }
    }
}