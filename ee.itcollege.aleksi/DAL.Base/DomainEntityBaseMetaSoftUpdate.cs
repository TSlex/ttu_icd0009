using System;
using ee.itcollege.aleksi.Contracts.DAL.Base;

namespace ee.itcollege.aleksi.DAL.Base
{
    public class DomainEntityBaseMetaSoftUpdate: DomainEntityMetadata, ISoftUpdateEntity
    {
        public Guid? MasterId { get; set; }
    }
}