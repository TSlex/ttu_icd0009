using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class FollowerDTO: DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid FollowerProfileId { get; set; } = default!;
    }
}