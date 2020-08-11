﻿using System.ComponentModel.DataAnnotations;

 namespace PublicApi.v1.Identity
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}