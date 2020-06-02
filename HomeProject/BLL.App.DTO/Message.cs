using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;
using DomainEntityBaseMetaSoftUpdateDelete = BLL.App.DTO.Base.DomainEntityBaseMetaSoftUpdateDelete;

namespace BLL.App.DTO
{
    public class Message : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(MessageValue), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [MaxLength(3000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string MessageValue { get; set; } = default!;

        [Display(Name = nameof(MessageDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public DateTime MessageDateTime { get; set; } = DateTime.Now;

        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        public Profile? Profile { get; set; }

        [Display(Name = nameof(ChatRoomId), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ChatRoomId { get; set; } = default!;

        [Display(Name = nameof(ChatRoom), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        public ChatRoom? ChatRoom { get; set; }
    }
}