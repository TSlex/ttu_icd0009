using System;

namespace Domain
{
    public class Message
    {
        public int MessageId { get; set; }
        
        public string MessageValue { get; set; }
        
        public DateTime MessageDateTime { get; set; }
    }
}