using System;

namespace Contracts.DAL.Base
{
    public interface IDomainEntityMetadata
    {
        string? CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        string? DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}