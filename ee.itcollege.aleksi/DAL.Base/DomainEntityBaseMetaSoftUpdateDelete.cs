using System;
using ee.itcollege.aleksi.Contracts.DAL.Base;

namespace ee.itcollege.aleksi.DAL.Base
{
    public class DomainEntityBaseMetaSoftUpdateDelete: DomainEntityBaseMetadata, ISoftUpdateEntity, ISoftDeleteEntity
    {
        public Guid? MasterId { get; set; }
    }
}