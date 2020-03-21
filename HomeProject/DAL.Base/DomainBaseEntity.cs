using System;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public class DomainBaseEntity : IDomainBaseEntity
    {
        public virtual Guid Id { get; set; }
    }
}