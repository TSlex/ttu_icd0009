using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using ee.itcollege.aleksi.DAL.Base;

namespace BLL.App.DTO
{
    public class Subject : DomainEntityBaseMetaSoftUpdateDelete
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

        public ICollection<StudentSubject>? StudentSubjects { get; set; }

        public ICollection<HomeWork>? HomeWorks { get; set; }
    }
    
    public class SubjectTeacherDetails : DomainEntityBaseMetaSoftUpdateDelete
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

        public ICollection<StudentSubject>? StudentSubjects { get; set; }

        public ICollection<HomeWork>? HomeWorks { get; set; }
    }
    
    public class SubjectDTO
    {
        public Guid Id { get; set; } = default!;
        
        public string SubjectTitle { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;
        public string SemesterTitle { get; set; } = default!;
        public string TeacherName { get; set; } = default!;
    }
    
    public class SubjectDetailsDTO: SubjectDTO
    {
        public int StudentsCount { get; set; }
    }
    
    public class SubjectStudentDetailsDTO: SubjectDTO
    {
        public int StudentsCount { get; set; }
        public int Grade { get; set; }
        public double HomeWorksGrade { get; set; }
        
        public Guid StudentSubjectId { get; set; }
        
        public bool IsAccepted { get; set; }
        public bool IsEnrolled { get; set; }

        public ICollection<SubjectStudentDetailsHomeworkDTO>? Homeworks { get; set; }
    }
    
    public class SubjectTeacherDetailsDTO: SubjectDTO
    {
        
        public int AcceptedStudentsCount { get; set; }
        public int NotAcceptedStudentsCount { get; set; }
        
        public int NotGradedCount { get; set; }
        
        public int FailedCount { get; set; }
        public int PassedCount { get; set; }
        
        public int Score1Count { get; set; }
        public int Score2Count { get; set; }
        public int Score3Count { get; set; }
        public int Score4Count { get; set; }
        public int Score5Count { get; set; }
        
        public ICollection<SubjectTeacherDetailsHomeworkDTO>? Homeworks { get; set; }
    }

    public class SubjectStudentDetailsHomeworkDTO
    {
        public Guid Id { get; set; } = default!;
        public Guid? StudentHomeworkId { get; set; }

        public string Title { get; set; } = default!;
        public DateTime? Deadline { get; set; }

        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; }
        
        public bool IsStarted { get; set; }
        
        public int Grade { get; set; }
    }
    
    public class SubjectTeacherDetailsHomeworkDTO
    {
        public Guid Id { get; set; } = default!;
        
        public string Title { get; set; } = default!;
        public DateTime? Deadline { get; set; }
        
        public double AverageGrade { get; set; }
    }
}

