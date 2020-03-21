using System;
using Contracts.DAL.Base;

namespace DAL.Base
{
    
    
    
    public abstract class DomainEntity: DomainBaseEntity, IDomainEntityMetadata
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        
        public string DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}