using System;

namespace Contracts.DAL.Base
{
    public interface IDomainEntity : IDomainEntity<string>
    {
        
    }
    
    public interface IDomainEntity<TKey> : IDomainBaseEntity<TKey>, IDomainEntityMetadata
    where TKey: IComparable
    {
        
    }
}