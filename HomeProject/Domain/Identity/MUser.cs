﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    [Table("User")]
    public class MUser : IdentityUser<Guid>, IDomainEntityBaseMetadata, ISoftDeleteEntity
    {
        public override Guid Id { get; set; } = default!;

        public DateTime RegistrationDateTime { get; set; } = DateTime.UtcNow;

        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}