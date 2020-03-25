using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class MessageDTO: DomainEntity
    {
        [MaxLength(2000)] public string MessageValue { get; set; } = default!;
        
        public DateTime MessageDateTime { get; set; } = DateTime.Now;
        
        [MaxLength(36)] public string ProfileId { get; set; } = default!;

        [MaxLength(36)] public string ChatRoomId { get; set; } = default!;
    }
}