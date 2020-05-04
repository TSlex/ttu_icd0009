using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Response
{
    public class JwtResponseDTO
    {
        [Required]
        public string Token { get; set; } = default!;
        [Required]
        public string Status { get; set; } = default!;
    }
}