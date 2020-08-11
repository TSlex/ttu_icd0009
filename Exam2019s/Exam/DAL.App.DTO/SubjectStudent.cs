﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.App.DTO.Identity;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class StudentSubject: DomainEntityBaseMetadata
    {
        [Required]
        public Guid StudentId { get; set; } = default!;
        public AppUser? Student { get; set; }
        
        [Required]
        public Guid SubjectId { get; set; } = default!;
        public Subject? Subject { get; set; }

        [Required] [Range(-1, 5)] public int Grade { get; set; } = -1;
        
        public bool IsAccepted { get; set; } = false;
        
        public ICollection<StudentHomeWork>? StudentHomeWorks { get; set; }
    }
}