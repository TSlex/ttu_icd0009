using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace Domain
{
    public class StudentHomeWork : DomainEntityBaseMetadata
    {
        [Required] public Guid HomeWorkId { get; set; } = default!;
        public HomeWork? HomeWork { get; set; }

        [Required] public Guid StudentSubjectId { get; set; } = default!;
        public StudentSubject? StudentSubject { get; set; }

        [Required] [Range(-1, 5)] public int Grade { get; set; } = -1;

        [Required] [MaxLength(4096)] public string? StudentAnswer { get; set; }

        public DateTime? AnswerDateTime { get; set; }

        public bool IsChecked { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
    }
}