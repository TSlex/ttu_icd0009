using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatRoomDTO
    {
        public string ChatRoomTitle { get; set; } = default!;

        public string? ChatRoomImageUrl { get; set; }
        public Guid? ChatRoomImageId { get; set; }

        public string? LastMessageValue { get; set; }
        public DateTime? LastMessageDateTime { get; set; }
    }
}