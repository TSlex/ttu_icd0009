using System;
using System.Collections.Generic;

namespace PublicApi.v1
{
    public class SubjectDTO
    {
        public Guid Id { get; set; } = default!;
        
        public string SubjectTitle { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;
        public string SemesterTitle { get; set; } = default!;
        public string TeacherName { get; set; } = default!;
    }
    
    public class SubjectDetails: SubjectDTO
    {
        public int StudentsCount { get; set; }
    }
    
    public class SubjectStudentDetails: SubjectDTO
    {
        public int StudentsCount { get; set; }
        public int Grade { get; set; }
        public double HomeWorksGrade { get; set; }
        
        public bool IsAccepted { get; set; }
        public bool IsEnrolled { get; set; }

        public ICollection<SubjectStudentDetailsHomework> Homeworks { get; set; }
    }
    
    public class SubjectTeacherDetails: SubjectDTO
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
        
        public ICollection<SubjectTeacherDetailsHomework> Homeworks { get; set; }
    }

    public class SubjectStudentDetailsHomework
    {
        public Guid Id { get; set; } = default!;
        
        public string Title { get; set; } = default!;
        public DateTime? Deadline { get; set; }

        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; }
        
        public bool IsStarted { get; set; }
        
        public int Grade { get; set; }
    }
    
    public class SubjectTeacherDetailsHomework
    {
        public Guid Id { get; set; } = default!;
        
        public string Title { get; set; } = default!;
        public DateTime? Deadline { get; set; }
        
        public double AverageGrade { get; set; }
    }
}