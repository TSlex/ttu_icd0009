using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using ee.itcollege.aleksi.DAL.Base;

namespace BLL.App.DTO.Base
{
    public class DomainEntityBaseMetaSoftUpdateDelete: DomainEntityBaseMetadata, ISoftUpdateEntity, ISoftDeleteEntity
    {
        [Display(Name = nameof(MasterId), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public Guid? MasterId { get; set; }
    }
}