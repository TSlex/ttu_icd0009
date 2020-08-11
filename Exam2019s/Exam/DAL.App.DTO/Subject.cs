using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.App.DTO.Identity;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class Subject : DomainEntityBaseMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string SubjectTitle { get; set; } = default!;

        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string SubjectCode { get; set; } = default!;

        [Required] public Guid TeacherId { get; set; } = default!;
        public AppUser? Teacher { get; set; }

        [Required] public Guid SemesterId { get; set; } = default!;
        public Semester? Semester { get; set; }

        public ICollection<StudentSubject>? StudentSubject { get; set; }

        public ICollection<HomeWork>? HomeWorks { get; set; }
    }
}