﻿using System.ComponentModel.DataAnnotations;

 namespace PublicApi.v1.Identity
{
    public class EmailDTO
    {
        [Required]
        [EmailAddress]
        public string CurrentEmail { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string NewEmail { get; set; } = default!;
    }
}