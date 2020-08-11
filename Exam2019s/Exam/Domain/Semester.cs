using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace Domain
{
    public class Semester: DomainEntityBaseMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Title { get; set; } = default!;
        
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Code { get; set; } = default!;
    }
}