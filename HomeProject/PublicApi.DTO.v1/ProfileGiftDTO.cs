using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ProfileGiftDTO : DomainEntityBaseMetadata
    {
        public string Username { get; set; } = default!;
        public string? FromUsername { get; set; }

        public Guid? ImageId { get; set; } = default!;

        public DateTime GiftDateTime { get; set; } = default!;

        public int Price { get; set; }

        public string? Message { get; set; }
    }

    public class ProfileGiftCreateDTO
    {
        public string Username { get; set; } = default!;
        public string? FromUsername { get; set; }

        public string GiftCode { get; set; } = default!;

        public string? Message { get; set; }
    }

    public class ProfileGiftAdminDTO : DomainEntityBaseMetaSoftDelete
    {
        public Guid ProfileId { get; set; } = default!;

        public Guid GiftId { get; set; } = default!;

        public DateTime GiftDateTime { get; set; } = default!;

        public int Price { get; set; }

        public Guid? FromProfileId { get; set; }

        public string? Message { get; set; }
    }
}