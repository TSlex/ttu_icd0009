using System;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;

namespace BLL.App.DTO.Base
{
    public abstract class DomainEntityMetadata: IDomainEntityMetadata
    {
        [Display(Name = nameof(CreatedBy), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? CreatedBy { get; set; }
        
        [Display(Name = nameof(CreatedAt), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public DateTime CreatedAt { get; set; } = default!;
        
        [Display(Name = nameof(ChangedBy), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? ChangedBy { get; set; }
        
        [Display(Name = nameof(ChangedAt), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public DateTime ChangedAt { get; set; } = default!;
        
        [Display(Name = nameof(DeletedBy), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public string? DeletedBy { get; set; }
        
        [Display(Name = nameof(DeletedAt), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public DateTime? DeletedAt { get; set; }
    }
}