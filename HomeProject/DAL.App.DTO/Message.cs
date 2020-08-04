using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class Message: DomainEntityBaseMetaSoftUpdateDelete
    {
        [MaxLength(3000)] public string MessageValue { get; set; } = default!;
        
        public DateTime MessageDateTime { get; set; } = DateTime.Now;
        
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid ChatRoomId { get; set; } = default!;
        public ChatRoom? ChatRoom { get; set; }
    }
}