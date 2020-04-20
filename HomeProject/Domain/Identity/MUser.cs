using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    [Table("User")]
    public class MUser: IdentityUser<Guid>, IDomainEntityBaseMetadata
    {
        [MaxLength(36)] public override Guid Id { get; set; } = default!;
        
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}