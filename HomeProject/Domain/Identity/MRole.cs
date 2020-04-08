using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{    
    [Table("UserRole")]
    public class MRole: IdentityRole<Guid>
    {
        [MaxLength(36)] public override Guid Id { get; set; } = default!;
    }
}