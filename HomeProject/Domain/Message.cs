using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Message: DomainEntityMetadata
    {
        [MaxLength(2000)] public string MessageValue { get; set; } = default!;
        
        public DateTime MessageDateTime { get; set; } = DateTime.Now;
        
        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        [MaxLength(36)] public string ChatRoomId { get; set; } = default!;
        public ChatRoom? ChatRoom { get; set; }
    }
}