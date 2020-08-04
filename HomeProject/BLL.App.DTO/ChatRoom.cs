using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;
using DomainEntityBaseMetaSoftUpdateDelete = BLL.App.DTO.Base.DomainEntityBaseMetaSoftUpdateDelete;

namespace BLL.App.DTO
{
    public class ChatRoom : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(ChatRoomTitle), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string ChatRoomTitle { get; set; } = default!;

        [Display(Name = nameof(LastMessageValue), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        [MaxLength(3000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? LastMessageValue { get; set; }

        [Display(Name = nameof(LastMessageDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        public DateTime? LastMessageDateTime { get; set; }

        [Display(Name = nameof(ChatRoomImageUrl), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ChatRoomImageUrl { get; set; }

        [Display(Name = nameof(ChatRoomImageId), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        public Guid? ChatRoomImageId { get; set; }

        [Display(Name = nameof(ChatMembers), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        public ICollection<ChatMember>? ChatMembers { get; set; }

        [Display(Name = nameof(Messages), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        public ICollection<Message>? Messages { get; set; }
    }
}