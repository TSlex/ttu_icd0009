using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Message: DomainEntityBaseMetadata
    {
        [MaxLength(3000)] public string MessageValue { get; set; } = default!;
        
        public DateTime MessageDateTime { get; set; } = DateTime.Now;
        
        public Guid ProfileId { get; set; } = default!;
        public ProfileFull? Profile { get; set; }
        
        public Guid ChatRoomId { get; set; } = default!;
        public ChatRoom? ChatRoom { get; set; }
    }
}