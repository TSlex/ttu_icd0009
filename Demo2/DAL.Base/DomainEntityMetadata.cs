using System;
using System.ComponentModel;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public abstract class DomainEntityMetadata : IDomainEntityMetadata
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
        
//        public string? DeletedBy { get; set; }
//        public DateTime? DeletedAt { get; set; }
    }
}