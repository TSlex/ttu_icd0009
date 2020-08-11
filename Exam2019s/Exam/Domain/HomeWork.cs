using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class HomeWork: DomainEntityBaseMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Title { get; set; } = default!;
        
        [Required]
        [MaxLength(4096)]
        public string? Description { get; set; }
        
        public DateTime? Deadline { get; set; }
        
        public Guid SubjectId { get; set; } = default!;
        public Subject? Subject { get; set; }
    }
}