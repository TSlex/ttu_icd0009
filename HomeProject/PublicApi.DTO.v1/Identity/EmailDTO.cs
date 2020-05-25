using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1.Identity
{
    public class EmailDTO
    {
        [DataType(DataType.EmailAddress)]
        public string CurrentEmail { get; set; } = default!;
        
        [DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; } = default!;
    }
}