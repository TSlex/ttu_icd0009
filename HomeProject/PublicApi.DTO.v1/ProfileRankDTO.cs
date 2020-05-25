using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ProfileRankAdminDTO: DomainEntityBaseMetaSoftDelete
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid RankId { get; set; } = default!;
    }
}