using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatRoomGetDTO
    {
        public Guid Id { get; set; } = default!;
        public string ChatRoomTitle { get; set; } = default!;

        public string? ChatRoomImageUrl { get; set; }
        public Guid? ChatRoomImageId { get; set; }

        public string? LastMessageValue { get; set; }
        public DateTime? LastMessageDateTime { get; set; }
    }

    public class ChatRoomEditDTO
    {
        [Display(Name = nameof(Id), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid Id { get; set; }
        
        [Display(Name = nameof(ChatRoomTitle), ResourceType = typeof(Resourses.BLL.App.DTO.ChatRooms.ChatRooms))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string ChatRoomTitle { get; set; } = default!;
    }

    public class ChatRoomAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
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
    }
}