using System;
using ee.itcollege.aleksi.Contracts.DAL.Base;

namespace ee.itcollege.aleksi.DAL.Base
{
    public class DomainEntityBase : IDomainEntityBase
    {
        public virtual Guid Id { get; set; }
    }
}