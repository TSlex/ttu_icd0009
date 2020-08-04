using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.Contracts.DAL.Base;

namespace BLL.App.DTO.Base
{
    public class DomainEntityBaseMetaSoftUpdate: BLL.App.DTO.Base.DomainEntityMetadata, ISoftUpdateEntity
    {
        [Display(Name = nameof(MasterId), ResourceType = typeof(Resourses.BLL.App.DTO.Common))]
        public Guid? MasterId { get; set; }
    }
}