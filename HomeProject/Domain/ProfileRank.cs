﻿using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class ProfileRank: DomainEntity
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid RankId { get; set; } = default!;
        public Rank? Rank { get; set; }
    }
}