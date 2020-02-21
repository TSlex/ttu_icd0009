using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppUser: IdentityUser
    {
        [MaxLength(36)] public override string Id { get; set; } = default!;

        public ICollection<Author> Authors { get; set; }

//        [MaxLength(256)] [MinLength(1)] public string FirstName { get; set; } = default!;
//        [MaxLength(256)] [MinLength(1)] public string LastName { get; set; } = default!;
    }
}