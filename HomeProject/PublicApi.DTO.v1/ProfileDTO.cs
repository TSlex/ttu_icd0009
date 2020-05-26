using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Identity;

namespace PublicApi.DTO.v1
{
    public class ProfileDTO
    {
        public DateTime? LastLoginDateTime { get; set; }

        public string? UserName { get; set; }
        public string? ProfileFullName { get; set; }

        public string? ProfileWorkPlace { get; set; }

        public string? ProfileStatus { get; set; }
        public string? ProfileAbout { get; set; }

        public Guid? ProfileAvatarId { get; set; }

        public ProfileGender ProfileGender { get; set; }
        public string? ProfileGenderOwn { get; set; }

        public int FollowersCount { get; set; } = 0;
        public int FollowedCount { get; set; } = 0;
        public int PostsCount { get; set; } = 0;

        public int Experience { get; set; } = 0;

        public bool IsUserBlocked { get; set; }
        public bool IsUserFollows { get; set; }
        public bool IsUserBlocks { get; set; }
    }

    public class ProfileAdminDTO : MUser
    {
        public DateTime? LastLoginDateTime { get; set; }

        [MinLength(1)] [MaxLength(100)] public string? ProfileFullName { get; set; }

        [MaxLength(300)] public string? ProfileWorkPlace { get; set; }

        [MaxLength(300)] public string? ProfileStatus { get; set; }

        public Guid? ProfileAvatarId { get; set; }

        [MaxLength(1000)] public string? ProfileAbout { get; set; }

        [Range(0, int.MaxValue)] public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;

        [MaxLength(20)] public string? ProfileGenderOwn { get; set; }

        public int FollowersCount { get; set; }
        public int FollowedCount { get; set; }
        public int PostsCount { get; set; }

        public int Experience { get; set; }
        
        public string? Password { get; set; }
    }
}