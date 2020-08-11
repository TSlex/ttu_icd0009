﻿using System.ComponentModel.DataAnnotations;

 namespace PublicApi.v1.Identity
{
    public class PasswordDTO
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;
    }
}