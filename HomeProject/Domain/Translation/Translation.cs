using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain.Translation
{
    public class Translation : DomainEntityBaseMetadata
    {
        [MaxLength(5)] public string Culture { get; set; } = default!;
        [MaxLength(10240)] public string Value { get; set; } = default!;

        public Guid LangStringId { get; set; } = default!;
        public LangString LangString { get; set; } = default!;
    }
}