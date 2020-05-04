using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Response
{
    public class OkResponseDTO
    {
        [Required]
        public string Status { get; set; } = default!;
    }
}