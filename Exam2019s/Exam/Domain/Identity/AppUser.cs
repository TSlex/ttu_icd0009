using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppUser : IdentityUser<Guid>, IDomainEntityBaseMetadata, ISoftDeleteEntity
    {
        public override Guid Id { get; set; } = default!;
        
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        [PersonalData]
        // ReSharper disable once MemberCanBePrivate.Global
        public string FirstName { get; set; } = default!;

        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        [PersonalData]
        // ReSharper disable once MemberCanBePrivate.Global
        public string LastName { get; set; } = default!;

        public string FirstLastName => FirstName + " " + LastName;
        public string LastFirstName => LastName + " " + FirstName;
        
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        // actual data
        public Subject? TeacherSubject { get; set; }
        public ICollection<StudentSubject>? ParticipationSubjects { get; set; }
    }
}