using System;

namespace Contracts.DAL.Base
{
    public interface IDomainEntityBase : IDomainEntityBase<Guid>
    {
    }
    
    public interface IDomainEntityBase<TKey> 
        where TKey : struct, IComparable
    {
        TKey Id { get; set; }
    }
}