using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class RankDTO: DomainEntityBaseMetadata
    {
        [MaxLength(100)] public string RankTitle { get; set; } = default!;
        [MaxLength(300)] public string? RankDescription { get; set; }
    }
}