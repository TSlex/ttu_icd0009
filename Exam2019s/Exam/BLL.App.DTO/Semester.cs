using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace BLL.App.DTO
{
    public class Semester: DomainEntityBaseMetaSoftUpdateDelete
    {
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Title { get; set; } = default!;
        
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Code { get; set; } = default!;
        
        public ICollection<Subject>? Subjects { get; set; }
    }
    
    public class SemesterDTO
    {
        public Guid Id { get; set; } = default!;

        public string Title { get; set; } = default!;
        public ICollection<SemesterSubjectDTO>? Subjects { get; set; }
    }

    public class SemesterSubjectDTO
    {
        public Guid Id { get; set; } = default!;

        public int Grade { get; set; }

        public string SubjectTitle { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;
        public string TeacherName { get; set; } = default!;

        public bool IsAccepted { get; set; }
    }
}