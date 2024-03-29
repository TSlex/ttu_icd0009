﻿using System;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class ProfileRank: DomainEntityBaseMetaSoftDelete
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        public Guid RankId { get; set; } = default!;
        public Rank? Rank { get; set; }
    }
}