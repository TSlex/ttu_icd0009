﻿using System;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public class DomainEntityBaseMetaSoftUpdateDelete: DomainEntityBaseMetadata, ISoftUpdateEntity, ISoftDeleteEntity
    {
        public Guid? MasterId { get; set; }
    }
}