using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Identity;
using ee.itcollege.aleksi.DAL.Base;

namespace Domain
{
    public class Subject: DomainEntityBaseMetadata
    {
        [Required]
        public string SubjectTitle { get; set; } = default!;
        
        [Required]
        public string SubjectCode { get; set; } = default!;

        [Required]
        public Guid TeacherId { get; set; } = default!;
        public AppUser? Teacher { get; set; }
        
        public ICollection<SubjectStudent>? SubjectStudent { get; set; }
        
        public ICollection<HomeWork>? HomeWorks { get; set; }
    }
}