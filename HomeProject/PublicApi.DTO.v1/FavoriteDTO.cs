using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class FavoriteDTO: DomainEntity
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid PostId { get; set; } = default!;
    }
}