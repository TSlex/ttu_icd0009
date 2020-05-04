using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = default!;
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;
    }
}