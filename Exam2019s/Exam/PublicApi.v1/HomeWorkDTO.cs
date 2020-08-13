using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO;

namespace PublicApi.v1
{
    public class HomeWorkDetailsDTO
    {
        public Guid Id { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        public string SubjectTitle { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;
        public Guid SubjectId { get; set; } = default!;

        public DateTime? Deadline { get; set; }
        
        public ICollection<StudentHomeWork> StudentHomeWorks { get; set; }
    }
    
    public class StudentHomeWork
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

    public class HomeWorkDTO
    {
        public Guid Id { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        public string SubjectTitle { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;
        public Guid SubjectId { get; set; } = default!;

        public DateTime? Deadline { get; set; }
    }

    public class HomeWorkPostDTO
    {
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Title { get; set; } = default!;

        [Required] public Guid SubjectId { get; set; } = default!;

        [MaxLength(4096)] public string? Description { get; set; }

        [DataType(DataType.DateTime)] public DateTime? Deadline { get; set; }
    }

    public class HomeWorkPutDTO
    {
        [Required] public Guid Id { get; set; } = default!;

        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Title { get; set; } = default!;

        [MaxLength(4096)] public string? Description { get; set; }

        [DataType(DataType.DateTime)] public DateTime? Deadline { get; set; }
    }
}