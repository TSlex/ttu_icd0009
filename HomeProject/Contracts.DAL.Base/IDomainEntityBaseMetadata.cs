using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts.DAL.Base
{
    public interface IDomainEntityBaseMetadata : IDomainEntityBaseMetadata<Guid>
    { 
    }

    public interface IDomainEntityBaseMetadata<TKey> : IDomainEntityBase<TKey>, IDomainEntityMetadata
        where TKey : struct, IComparable
    {
    }
}