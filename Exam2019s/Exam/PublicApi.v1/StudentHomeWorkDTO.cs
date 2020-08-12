using System;

namespace PublicApi.v1
{
    public class StudentHomeWorkDTO
    {
        public Guid SubjectId { get; set; } = default!;
        public Guid HomeWorkId { get; set; } = default!;

        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; }

        public DateTime? Deadline { get; set; }
        public int Grade { get; set; }
    }

    public class StudentHomeWorkDetailsDTO
    {
        public Guid Id { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }

        public string SubjectTitle { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;

        public Guid SubjectId { get; set; } = default!;
        public Guid HomeWorkId { get; set; } = default!;

        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; }

        public string? StudentAnswer { get; set; }
        public DateTime? AnswerDateTime { get; set; }

        public int Grade { get; set; }
    }

    public class StudentHomeWorkPostDTO
    {
        public string? StudentAnswer { get; set; }
        public Guid StudentSubjectId { get; set; } = default!;
        public Guid HomeWorkId { get; set; } = default!;
    }

    public class StudentHomeWorkPutDTO
    {
        public Guid Id { get; set; } = default!;

        public string? StudentAnswer { get; set; }
    }

    public class StudentHomeWorkTeacherSubmitDTO
    {
        public Guid Id { get; set; } = default!;

        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; }
        
        public int Grade { get; set; }
    }
}