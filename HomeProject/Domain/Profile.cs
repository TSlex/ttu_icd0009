using System;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Profile : IdentityUser 
    {
        public DateTime RegistrationDateTime { get; set; }
        public DateTime LastLoginDateTime { get; set; }

        public string ProfileStatus { get; set; }
        
        public string ProfileAvatarUrl { get; set; }

        public string ProfileAbout { get; set; }

        public int FollowersCount { get; set; }
        public int FollowedCount { get; set; }
        public int PostsCount { get; set; }
    }
}