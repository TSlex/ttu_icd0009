using System;

namespace Domain
{
    public class Message
    {
        public string MessageId { get; set; }
        
        public string MessageValue { get; set; }
        
        public DateTime MessageDateTime { get; set; }
        
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}