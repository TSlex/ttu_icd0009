using System;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO;
using Domain.Enums;

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
        
        public string? ProfileAvatarUrl { get; set; }
        public Guid? ProfileAvatarId { get; set; }
        public Image? ProfileAvatar { get; set; }
        
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
}