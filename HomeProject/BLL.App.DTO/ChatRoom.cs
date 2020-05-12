using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class ChatRoom : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(ChatRoomTitle), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        [MaxLength(100)]
        public string ChatRoomTitle { get; set; } = default!;

        [Display(Name = nameof(LastMessageValue), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        [MaxLength(3000)]
        public string? LastMessageValue { get; set; }

        [Display(Name = nameof(LastMessageDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        public DateTime? LastMessageDateTime { get; set; }

        [Display(Name = nameof(ChatRoomImageUrl), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        [MaxLength(300)]
        public string? ChatRoomImageUrl { get; set; }

        [Display(Name = nameof(ChatRoomImageId), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        public Guid? ChatRoomImageId { get; set; }

        [Display(Name = nameof(ChatMembers), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        public ICollection<ChatMember>? ChatMembers { get; set; }

        [Display(Name = nameof(Messages), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        public ICollection<Message>? Messages { get; set; }
    }
}