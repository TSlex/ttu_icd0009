using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class PasswordDTO
    {
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = default!;
        
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;
    }
}