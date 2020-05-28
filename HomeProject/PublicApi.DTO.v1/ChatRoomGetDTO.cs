using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatRoomGetDTO
    {
        public Guid Id { get; set; } = default!;
        public string ChatRoomTitle { get; set; } = default!;

        public string? ChatRoomImageUrl { get; set; }
        public Guid? ChatRoomImageId { get; set; }

        public string? LastMessageValue { get; set; }
        public DateTime? LastMessageDateTime { get; set; }
    }

    public class ChatRoomEditDTO
    {
        public Guid Id { get; set; }
        [MaxLength(100)] public string ChatRoomTitle { get; set; } = default!;
    }

    public class ChatRoomAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(100)] public string ChatRoomTitle { get; set; } = default!;

        [MaxLength(300)] public string? ChatRoomImageUrl { get; set; }
        public Guid? ChatRoomImageId { get; set; }
    }
}