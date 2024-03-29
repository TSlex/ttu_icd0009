﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using MUser = DAL.App.DTO.Identity.MUser;

namespace DAL.App.DTO
{
    public class Profile : MUser
    {
        public DateTime? LastLoginDateTime { get; set; }

        [MinLength(1)] [MaxLength(100)] public string? ProfileFullName { get; set; }

        [MaxLength(300)] public string? ProfileWorkPlace { get; set; }
        
        [MaxLength(300)] public string? ProfileStatus { get; set; }

//        [MaxLength(300)] public string? ProfileAvatarUrl { get; set; }
        
        public Guid? ProfileAvatarId { get; set; }
        public Image? ProfileAvatar { get; set; }

        [MaxLength(1000)] public string? ProfileAbout { get; set; }

        [Range(0, int.MaxValue)] public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;
        
        [MaxLength(20)]
        public string? ProfileGenderOwn { get; set; }

        public int FollowersCount { get; set; }
        public int FollowedCount { get; set; }
        public int PostsCount { get; set; }
        
        public int Experience { get; set; }
        
        public bool IsUserFollows { get; set; }
        public bool IsUserBlocks { get; set; }
        public bool IsUserBlocked { get; set; }

        //References
        //Black list
        [InverseProperty(nameof(BlockedProfile.Profile))]
        public ICollection<BlockedProfile>? BlockedProfiles { get; set; }

        [InverseProperty(nameof(BlockedProfile.BProfile))]
        public ICollection<BlockedProfile>? BlockedByProfiles { get; set; }

        //Black list
        [InverseProperty(nameof(Follower.Profile))]
        public ICollection<Follower>? Followers { get; set; } //profile followers list

        [InverseProperty(nameof(Follower.FollowerProfile))]
        public ICollection<Follower>? Followed { get; set; } //profile followed profiles list

        //Chat
        public ICollection<ChatMember>? ChatMembers { get; set; } //List of ChatRooms, where profile participate
        public ICollection<Message>? Messages { get; set; }

        public ICollection<Favorite>? Favorites { get; set; } //list of profile likes
        public ICollection<Post>? Posts { get; set; } //List of profile posts
        public ICollection<Comment>? Comments { get; set; } //List of profile comments

        public ICollection<ProfileGift>? ProfileGifts { get; set; } //List of profile gifts
        public ICollection<ProfileRank>? ProfileRanks { get; set; } //List of profile ranks
    }
}