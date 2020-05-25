using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace PublicApi.DTO.v1.Identity
{
    public class ProfileDataDTO
    {
        [MinLength(1)] [MaxLength(20)] public string Username { set; get; }

        [MinLength(1)] [MaxLength(100)] public string? ProfileFullName { get; set; }
        [MaxLength(300)] public string? ProfileWorkPlace { get; set; }
        [MaxLength(1000)] public string? ProfileAbout { get; set; }

        [DataType(DataType.PhoneNumber)] public string? PhoneNumber { get; set; }

        [Range(0, int.MaxValue)] public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;

        [MaxLength(20)] public string? ProfileGenderOwn { get; set; }
    }
}