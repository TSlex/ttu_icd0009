using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.v1.Response
{
    public class ErrorResponseDTO
    {
        [Required]
        public IList<string> Errors { get; set; }

        public ErrorResponseDTO(params string[] errors)
        {
            Errors = errors;
        }
    }
}