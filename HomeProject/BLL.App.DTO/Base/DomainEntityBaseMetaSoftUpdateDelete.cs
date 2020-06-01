using System;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;
using DAL.Base;

namespace BLL.App.DTO.Base
{
    public class DomainEntityBaseMetaSoftUpdateDelete: DomainEntityBaseMetadata, ISoftUpdateEntity, ISoftDeleteEntity
    {
        [Display(Name = nameof(MasterId), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public Guid? MasterId { get; set; }
    }
}