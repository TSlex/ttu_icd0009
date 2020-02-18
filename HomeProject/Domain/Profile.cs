﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Profile : IdentityUser 
    {
        public override string Id { get; set; }
        
        public DateTime RegistrationDateTime { get; set; }
        public DateTime LastLoginDateTime { get; set; }

        public string ProfileStatus { get; set; }
        
        public string ProfileAvatarUrl { get; set; }

        public string ProfileAbout { get; set; }

        public int FollowersCount { get; set; }
        public int FollowedCount { get; set; }
        public int PostsCount { get; set; }
        
        //References
        public ICollection<BlockedProfile> BlockedProfiles { get; set; } //profile black list
        public ICollection<BlockedProfile> BlockedByProfiles { get; set; }
        
        public ICollection<ChatMember> ChatMembers { get; set; } //List of ChatRooms, where profile participate
        public ICollection<Message> Messages { get; set; } 

        public ICollection<Follower> Followers { get; set; } //profile followers list
        public ICollection<Follower> Followed { get; set; } //profile followed profiles list
        
        public ICollection<Favorite> Favorites { get; set; } //list of profile likes
        public ICollection<Post> Posts { get; set; } //List of profile posts
        public ICollection<Comment> Comments { get; set; } //List of profile comments
        
        public ICollection<ProfileGift> ProfileGifts { get; set; } //List of profile gifts
        public ICollection<ProfileRank> ProfileRanks { get; set; } //List of profile ranks
    }
}