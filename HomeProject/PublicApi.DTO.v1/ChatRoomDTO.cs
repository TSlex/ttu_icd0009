using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatRoomDTO: DomainEntityBaseMetadata
    {
        [MaxLength(100)] public string ChatRoomTitle { get; set; } = default!;
        [MaxLength(100)] public string? LastMessageValue { get; set; }

        public DateTime? LastMessageDateTime { get; set; }
    }
}