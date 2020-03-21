using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts.DAL.Base
{
    public interface IDomainEntity : IDomainEntity<Guid>
    { 
    }

    public interface IDomainEntity<TKey> : IDomainBaseEntity<TKey>, IDomainEntityMetadata
        where TKey : struct, IComparable
    {
    }
}