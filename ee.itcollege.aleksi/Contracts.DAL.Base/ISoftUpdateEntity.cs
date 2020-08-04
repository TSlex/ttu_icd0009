using System;

namespace ee.itcollege.aleksi.Contracts.DAL.Base
{
    public interface ISoftUpdateEntity: ISoftUpdateEntity<Guid>
    {
        
    }
    
    public interface ISoftUpdateEntity<TKey>
        where TKey : struct, IComparable
    {
        TKey? MasterId { get; set; }
    }
}