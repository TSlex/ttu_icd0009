using System;
using System.ComponentModel.DataAnnotations;
using Domain.Identity;
using ee.itcollege.aleksi.DAL.Base;

namespace Domain
{
    public class SubjectStudent: DomainEntityBaseMetadata
    {
        [Required]
        public Guid SubjectId { get; set; } = default!;
        public Subject? Subject { get; set; }
        
        [Required]
        public Guid StudentId { get; set; } = default!;
        public AppUser? Student { get; set; }
    }
}