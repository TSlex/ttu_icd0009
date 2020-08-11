﻿using System.ComponentModel.DataAnnotations;

 namespace PublicApi.v1.Identity
{
    public class DeleteDTO
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}