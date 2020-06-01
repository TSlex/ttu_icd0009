using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using DomainEntityBaseMetaSoftUpdateDelete = BLL.App.DTO.Base.DomainEntityBaseMetaSoftUpdateDelete;

namespace BLL.App.DTO
{
    public class ChatMember : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(ChatRoomTitle), ResourceType = typeof(Resourses.BLL.App.DTO.ChatMembers.ChatMembers))]
        [MaxLength(100)]
        public string? ChatRoomTitle { get; set; }

        [Display(Name = nameof(ChatRoomId), ResourceType = typeof(Resourses.BLL.App.DTO.ChatMembers.ChatMembers))]
        public Guid ChatRoomId { get; set; } = default!;

        [Display(Name = nameof(ChatRoom), ResourceType = typeof(Resourses.BLL.App.DTO.ChatMembers.ChatMembers))]
        public ChatRoom? ChatRoom { get; set; }

        [Display(Name = nameof(ChatRoleId), ResourceType = typeof(Resourses.BLL.App.DTO.ChatMembers.ChatMembers))]
        public Guid ChatRoleId { get; set; } = default!;

        [Display(Name = nameof(ChatRole), ResourceType = typeof(Resourses.BLL.App.DTO.ChatMembers.ChatMembers))]
        public ChatRole? ChatRole { get; set; }

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.ChatMembers.ChatMembers))]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.ChatMembers.ChatMembers))]
        public Profile? Profile { get; set; }
    }
}