﻿using System.ComponentModel.DataAnnotations;

 namespace PublicApi.v1.Identity
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string FirstName { get; set; } = default!;
        
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string LastName { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}