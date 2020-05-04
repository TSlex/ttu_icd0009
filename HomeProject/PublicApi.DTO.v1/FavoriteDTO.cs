using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class FavoriteDTO: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public Guid PostId { get; set; } = default!;
    }

    public class FavoriteProfileDTO
    {
        public string UserName { get; set; }
        public string? ProfileAvatarUrl { get; set; }
    }
}