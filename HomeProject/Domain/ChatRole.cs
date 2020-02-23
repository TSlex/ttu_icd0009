using System.Collections.Generic;

namespace Domain
{
    public class ChatRole
    {
        public string RoleTitle { get; set; }
        
        public ICollection<ChatMember> ChatMembers { get; set; }
    }
}