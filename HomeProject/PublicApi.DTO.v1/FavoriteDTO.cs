using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class FavoriteProfileDTO
    {
        public string UserName { get; set; } = default!;
        public Guid? ProfileAvatarId { get; set; }
    }

    public class FavoriteAdminDTO : DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid PostId { get; set; } = default!;

        //what content user actually likes
        [MaxLength(100)] public string? PostTitle { get; set; }
        public Guid? PostImageId { get; set; }
        [MaxLength(100)] public string? PostDescription { get; set; }
    }
}