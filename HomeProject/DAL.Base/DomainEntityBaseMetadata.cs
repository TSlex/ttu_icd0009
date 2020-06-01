using System;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public abstract class DomainEntityBaseMetadata : IDomainEntityBaseMetadata
    {
        public Guid Id { get; set; } = default!;

        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = default!;

        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; } = default!;

        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}