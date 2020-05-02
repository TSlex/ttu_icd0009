using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.App.DTO.Identity;
using Domain.Enums;

namespace BLL.App.DTO
{
    public class ProfileFull : MUser
    {
        public DateTime? LastLoginDateTime { get; set; }

        [MinLength(1)] [MaxLength(100)] public string? ProfileFullName { get; set; }

        [MaxLength(300)] public string? ProfileWorkPlace { get; set; }
        
        [MaxLength(300)] public string? ProfileStatus { get; set; }

        [MaxLength(300)] public string? ProfileAvatarUrl { get; set; }

        [MaxLength(1000)] public string? ProfileAbout { get; set; }

        [Range(0, int.MaxValue)] public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;
        
        [MaxLength(20)]
        public string? ProfileGenderOwn { get; set; }

        public int FollowersCount { get; set; }
        public int FollowedCount { get; set; }
        public int PostsCount { get; set; }
        
        public int Experience { get; set; }

        //References
        //Black list
        public ICollection<BlockedProfile>? BlockedProfiles { get; set; }
        
        public ICollection<BlockedProfile>? BlockedByProfiles { get; set; }

        //Black list
        public ICollection<Follower>? Followers { get; set; } //profile followers list
        
        public ICollection<Follower>? Followed { get; set; } //profile followed profiles list

        //Chat
        public ICollection<ChatMember>? ChatMembers { get; set; } //List of ChatRooms, where profile participate
        public ICollection<Message>? Messages { get; set; }

        public ICollection<Favorite>? Favorites { get; set; } //list of profile likes
        public ICollection<Post>? Posts { get; set; } //List of profile posts
        public ICollection<Comment>? Comments { get; set; } //List of profile comments

        public ICollection<ProfileGift>? ProfileGifts { get; set; } //List of profile gifts
        public ICollection<ProfileRank>? ProfileRanks { get; set; } //List of profile ranks
        
        //TODO: split
        public bool IsUserFollows { get; set; }
        public bool IsUserBlocks { get; set; }
        public bool IsUserBlocked { get; set; }
    }

    public class ProfileLimited
    {
        public string UserName { get; set; } = default!;
        
        public DateTime? LastLoginDateTime { get; set; }
        
        public string? ProfileFullName { get; set; }
        public string? ProfileWorkPlace { get; set; }
        public string? ProfileStatus { get; set; }
        public string? ProfileAvatarUrl { get; set; }
        public string? ProfileAbout { get; set; }
        
        public int FollowersCount { get; set; }
        public int FollowedCount { get; set; }
        public int PostsCount { get; set; }
        
        public int Experience { get; set; }
        public ProfileRank Rank { get; set; } = default!;
    }

    public class ProfileEdit: MUser
    {
        [MinLength(1)] [MaxLength(100)] public string? ProfileFullName { get; set; }

        [MaxLength(300)] public string? ProfileWorkPlace { get; set; }
        
        [MaxLength(300)] public string? ProfileStatus { get; set; }

        [MaxLength(300)] public string? ProfileAvatarUrl { get; set; }

        [MaxLength(1000)] public string? ProfileAbout { get; set; }

        [Range(0, int.MaxValue)] public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;
        
        [MaxLength(20)]
        public string? ProfileGenderOwn { get; set; }
        
        public int Experience { get; set; }
        public string? Password { get; set; }
    }
}