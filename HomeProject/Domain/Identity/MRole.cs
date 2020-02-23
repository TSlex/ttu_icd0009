using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class MRole: IdentityRole
    {
        [MaxLength(36)] public override string Id { get; set; } = default!;
    }
}