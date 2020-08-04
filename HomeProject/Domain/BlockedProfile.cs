using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ee.itcollege.aleksi.DAL.Base;

using Domain.Translation;

namespace Domain
{
    public class BlockedProfile : DomainEntityBaseMetadata
    {
        // Profile who wants to block BProfile
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        // BProfile blocked by Profile
        public Guid BProfileId { get; set; } = default!;
        public Profile? BProfile { get; set; }

//        [MaxLength(200)] public string? Reason { get; set; } //filed by enum

        [MaxLength(200)] public string? ReasonId { get; set; } //filed by enum
        [MaxLength(200)] public LangString? Reason { get; set; } //filed by enum
    }
}