using System.ComponentModel.DataAnnotations;

namespace PublicApi.v1.Identity
{
    public class UserDataDTO
    {
        [Required]
        [MaxLength(300)]
        public string UserName { set; get; } = default!;

        [MinLength(1)]
        [MaxLength(100)]
        public string? FirstName { get; set; }
        
        [MinLength(1)]
        [MaxLength(100)]
        public string? LastName { get; set; }
        
        [Phone]
        [MaxLength(300)]
        public string? PhoneNumber { get; set; }
    }
}