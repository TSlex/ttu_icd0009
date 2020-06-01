using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    /// <summary>
    /// Get only
    /// </summary>
    public class GiftDTO
    {
        public string GiftName { get; set; } = default!;
        public string GiftCode { get; set; } = default!;

        public Guid? GiftImageId { get; set; }

        public int Price { get; set; }
    }

    /// <summary>
    /// Get only
    /// </summary>
    public class GiftsCountDTO
    {
        public int Count { get; set; }
    }
    
    public class GiftAdminDTO: DomainEntityBaseMetaSoftUpdateDelete
    {
        public Guid GiftNameId { get; set; } = default!;
        [MaxLength(100)] public string? GiftName { get; set; } = default!;

        [MaxLength(100)] public string GiftCode { get; set; } = default!;

        public Guid? GiftImageId { get; set; }

        public int Price { get; set; }
    }
}