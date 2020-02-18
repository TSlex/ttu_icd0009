using System;

namespace Domain
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        
        public string ChatRoomTitle { get; set; }
        public string LastMessageValue { get; set; }
        
        public DateTime LastMessageDateTime { get; set; }
    }
}