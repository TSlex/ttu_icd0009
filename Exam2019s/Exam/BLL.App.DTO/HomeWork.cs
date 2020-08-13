using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace BLL.App.DTO
{
    public class HomeWork: DomainEntityBaseMetaSoftUpdateDelete
    {
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Title { get; set; } = default!;
        
        [MaxLength(4096)]
        public string? Description { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? Deadline { get; set; }
        
        [Required]
        public Guid SubjectId { get; set; } = default!;
        public Subject? Subject { get; set; }

        public ICollection<StudentHomeWork>? StudentHomeWorks { get; set; }
    }
    
    public class HomeWorkDetailsDTO
    {
        public Guid Id { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        public string SubjectTitle { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;
        public Guid SubjectId { get; set; } = default!;

        public DateTime? Deadline { get; set; }
        
        public ICollection<StudentHomeWorkDTO>? StudentHomeWorks { get; set; }
    }
    
    public class StudentHomeWorkDTO
    {
        public Guid StudentSubjectId { get; set; } = default!;
        public Guid SubjectId { get; set; } = default!;
        public Guid HomeWorkId { get; set; } = default!;
        
        public string StudentName { get; set; } = default!;
        public string StudentCode { get; set; } = default!;
        
        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; }
        
        public int Grade { get; set; }
    }
}