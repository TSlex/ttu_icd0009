using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DAL.App.DTO.Identity
{
    public class AppUser
    {
        public Guid Id { get; set; } = default!;
        
        [MinLength(1)]
        [MaxLength(128)]
        [Required]
        // ReSharper disable once MemberCanBePrivate.Global
        public string FirstName { get; set; } = default!;

        [MinLength(1)]
        [MaxLength(128)]
        [Required]
        // ReSharper disable once MemberCanBePrivate.Global
        public string LastName { get; set; } = default!;
    }
}