using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class ProfileDeleteDTO
    {
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}