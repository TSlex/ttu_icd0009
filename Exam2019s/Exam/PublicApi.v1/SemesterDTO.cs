using System;
using System.Collections.Generic;

namespace PublicApi.v1
{
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