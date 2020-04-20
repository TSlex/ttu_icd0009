using System;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public class DomainEntityBase : IDomainEntityBase
    {
        public virtual Guid Id { get; set; }
    }
}