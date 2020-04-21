using System;
using Contracts.DAL.Base;

namespace BLL.App.DTO.Identity
{
    public class MUser: IDomainEntityBaseMetadata
    {
        public Guid Id { get; set; }
        
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string UserName { get; set; }
    }
}