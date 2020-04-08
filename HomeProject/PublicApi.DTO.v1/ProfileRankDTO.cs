using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ProfileRankDTO: DomainEntity
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid RankId { get; set; } = default!;
    }
}