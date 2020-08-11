﻿using System.ComponentModel.DataAnnotations;

 namespace PublicApi.v1.Identity
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(300)]
        public string Username { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}