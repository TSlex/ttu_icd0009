﻿using System.ComponentModel.DataAnnotations;

 namespace PublicApi.v1.Response
{
    public class OkResponseDTO
    {
        [Required]
        public string Status { get; set; } = default!;
    }
}