using System;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;

namespace BLL.App.DTO.Base
{
    public class DomainEntityBase : IDomainEntityBase
    {
        [Display(Name = nameof(Id), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public virtual Guid Id { get; set; }
    }
}