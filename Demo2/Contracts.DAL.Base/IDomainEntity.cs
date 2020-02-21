using System;

namespace Contracts.DAL.Base
{
    public interface IDomainEntity<TKey>
        where TKey : IComparable
    {
        TKey Id { get; set; }
    }

    public interface IDomainEntity : IDomainEntity<string>
    { 
    }
}