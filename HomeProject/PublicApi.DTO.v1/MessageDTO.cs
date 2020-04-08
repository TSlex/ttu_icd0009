using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class MessageDTO: DomainEntity
    {
        [MaxLength(2000)] public string MessageValue { get; set; } = default!;
        
        public DateTime MessageDateTime { get; set; } = DateTime.Now;
        
        public Guid ProfileId { get; set; } = default!;

        public Guid ChatRoomId { get; set; } = default!;
    }
}