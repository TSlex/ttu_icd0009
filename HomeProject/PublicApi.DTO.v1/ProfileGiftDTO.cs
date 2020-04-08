using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ProfileGiftDTO: DomainEntity
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid GiftId { get; set; } = default!;
    }
}