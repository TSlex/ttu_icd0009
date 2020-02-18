namespace Domain
{
    public class ChatMember
    {
        public int ChatMemberId { get; set; }
        
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public string ChatRoleId { get; set; }
        public ChatRole ChatRole { get; set; }
        
        public string ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}