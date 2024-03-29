﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public override Guid Id { get; set; } = default!;
    }
}