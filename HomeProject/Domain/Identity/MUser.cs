using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    [Table("User")]
    public class MUser: IdentityUser
    {
        [MaxLength(36)] public override string Id { get; set; } = default!;
        
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
    }
}