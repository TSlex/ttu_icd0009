using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class MessageGetDTO
    {
        public Guid Id { get; set; } = default!;
        public Guid ChatRoomId { get; set; } = default!;
        
        public Guid? ProfileAvatarId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string MessageValue { get; set; } = default!;

        public DateTime MessageDateTime { get; set; }
    }

    public class MessageCreateDTO
    {
        [Display(Name = nameof(ChatRoomId), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ChatRoomId { get; set; } = default!;
        
        [Display(Name = nameof(MessageValue), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [MaxLength(3000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string MessageValue { get; set; } = default!;
    }

    public class MessageEditDTO
    {
        [Display(Name = nameof(Id), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid Id { get; set; } = default!;

        [Display(Name = nameof(MessageValue), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [MaxLength(3000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string MessageValue { get; set; } = default!;
    }

    public class MessageAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
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

        [Display(Name = nameof(ChatRoomId), ResourceType = typeof(Resourses.BLL.App.DTO.Messages.Messages))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid ChatRoomId { get; set; } = default!;
    }
}