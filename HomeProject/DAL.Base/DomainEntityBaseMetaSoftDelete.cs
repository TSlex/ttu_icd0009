using System;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public class DomainEntityBaseMetaSoftDelete: DomainEntityBaseMetadata, ISoftDeleteEntity
    {
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}