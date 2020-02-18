using System;
using System.Collections.Generic;

namespace Domain
{
    public class ChatRoom
    {
        public string ChatRoomId { get; set; }
        
        public string ChatRoomTitle { get; set; }
        public string LastMessageValue { get; set; }
        
        public DateTime LastMessageDateTime { get; set; }
        
        public ICollection<ChatMember> ChatMembers { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}