using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace DAL.App.DTO.Identity
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

        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        
        public string? PhoneNumber { get; set; }
        
        public bool PhoneNumberConfirmed { get; set; } = default!;
        public bool LockoutEnabled { get; set; } = default!;
        public bool EmailConfirmed { get; set; } = default!;
        
        public int AccessFailedCount { get; set; } = default!;
    }
}