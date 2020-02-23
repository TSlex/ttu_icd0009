using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{    
    [Table("UserRole")]
    public class MRole: IdentityRole
    {
        [MaxLength(36)] public override string Id { get; set; } = default!;
    }
}