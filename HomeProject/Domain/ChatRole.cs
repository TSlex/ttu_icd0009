using System.Collections.Generic;

namespace Domain
{
    public class ChatRole
    {
        public int ChatRoleId { get; set; }
        public string RoleTitle { get; set; }
        
        public ICollection<ChatMember> ChatMembers { get; set; }
    }
}