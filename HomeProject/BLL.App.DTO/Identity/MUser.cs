using System;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;

namespace BLL.App.DTO.Identity
{
    public class MUser : IDomainEntityBaseMetadata
    {
        [Display(Name = nameof(Id), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public Guid Id { get; set; }

        [Display(Name = nameof(CreatedBy), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? CreatedBy { get; set; }

        [Display(Name = nameof(CreatedAt), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public DateTime CreatedAt { get; set; }

        [Display(Name = nameof(ChangedBy), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? ChangedBy { get; set; }

        [Display(Name = nameof(ChangedAt), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public DateTime ChangedAt { get; set; }

        [Display(Name = nameof(DeletedBy), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? DeletedBy { get; set; }

        [Display(Name = nameof(DeletedAt), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public DateTime? DeletedAt { get; set; }

        [Display(Name = nameof(UserName), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string UserName { get; set; } = default!;

        [Display(Name = nameof(Email), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string? Email { get; set; }

        [Display(Name = nameof(PhoneNumber), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string? PhoneNumber { get; set; }

        [Display(Name = nameof(PhoneNumberConfirmed), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = nameof(LockoutEnabled), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public bool LockoutEnabled { get; set; }

        [Display(Name = nameof(EmailConfirmed), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public bool EmailConfirmed { get; set; }

        [Display(Name = nameof(AccessFailedCount), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int AccessFailedCount { get; set; }
    }
}