﻿using System;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public abstract class DomainBaseEntity : IDomainBaseEntity
    {    
        [MaxLength(36)]
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
    }
}