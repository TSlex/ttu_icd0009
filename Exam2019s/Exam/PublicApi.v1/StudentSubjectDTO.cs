using System;

namespace PublicApi.v1
{
    public class StudentSubjectDTO
    {
        public Guid Id { get; set; } = default!;
        
        public bool IsAccepted { get; set; }
        
        public string StudentCode { get; set; } = default!;
        public string StudentName { get; set; } = default!;
        
        public Guid SubjectId { get; set; } = default!;
        
        public int Grade { get; set; }
    }
    
    public class StudentSubjectEditModelDTO
    {
        public Guid Id { get; set; } = default!;
        
        public bool IsAccepted { get; set; }
        
        public string StudentCode { get; set; } = default!;
        public string StudentName { get; set; } = default!;
        
        public Guid SubjectId { get; set; } = default!;
        
        public int Grade { get; set; }
    }
    
    public class StudentSubjectPutDTO
    {
        public Guid Id { get; set; } = default!;
        
        public bool IsAccepted { get; set; }
        
        public int Grade { get; set; }
    }

    public class StudentControlDTO
    {
        public Guid Id { get; set; } = default!;
        public Guid SubjectId { get; set; } = default!;
    }
}