using System;

namespace ee.itcollege.aleksi.Contracts.DAL.Base
{
    public interface ISoftDeleteEntity: ISoftDeleteEntity<Guid>
    {
        
    }
    
    public interface ISoftDeleteEntity<TKey>
        where TKey : struct, IComparable
    {
        string? DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}