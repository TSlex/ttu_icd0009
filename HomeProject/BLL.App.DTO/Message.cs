using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Message : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(MessageValue), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [MaxLength(3000)]
        public string MessageValue { get; set; } = default!;

        [Display(Name = nameof(MessageDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        public DateTime MessageDateTime { get; set; } = DateTime.Now;

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        public ProfileFull? Profile { get; set; }

        [Display(Name = nameof(ChatRoomId), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        public Guid ChatRoomId { get; set; } = default!;

        [Display(Name = nameof(ChatRoom), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        public ChatRoom? ChatRoom { get; set; }
    }
}