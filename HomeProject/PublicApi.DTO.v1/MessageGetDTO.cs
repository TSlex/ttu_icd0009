using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class MessageGetDTO
    {
        public Guid Id { get; set; } = default!;
        public Guid ChatRoomId { get; set; } = default!;
        
        public string UserName { get; set; } = default!;
        public string MessageValue { get; set; } = default!;
        
        public DateTime MessageDateTime { get; set; }
    }
    
    public class MessageCreateDTO
    {
        public Guid ChatRoomId { get; set; } = default!;
        [MaxLength(3000)] public string MessageValue { get; set; } = default!;
    }
    
    public class MessageEditDTO
    {
        public Guid Id { get; set; } = default!;

        [MaxLength(3000)] public string MessageValue { get; set; } = default!;
    }
}