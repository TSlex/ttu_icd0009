using System;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public class DomainEntity: IDomainEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}