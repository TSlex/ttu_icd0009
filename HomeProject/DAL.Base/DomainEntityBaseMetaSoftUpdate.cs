using System;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public class DomainEntityBaseMetaSoftUpdate: DomainEntityMetadata, ISoftUpdateEntity
    {
        public Guid? MasterId { get; set; }
    }
}