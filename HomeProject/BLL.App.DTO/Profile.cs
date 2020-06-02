using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.App.DTO.Identity;
using Domain.Enums;

namespace BLL.App.DTO
{
    public class Profile : MUser
    {
        [Display(Name = nameof(LastLoginDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public DateTime? LastLoginDateTime { get; set; }

        [Display(Name = nameof(ProfileFullName), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MinLength(1, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MinLength")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileFullName { get; set; }

        [Display(Name = nameof(ProfileWorkPlace), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileWorkPlace { get; set; }

        [Display(Name = nameof(ProfileStatus), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileStatus { get; set; }

        [Display(Name = nameof(ProfileAvatarId), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public Guid? ProfileAvatarId { get; set; }

        [Display(Name = nameof(ProfileAvatar), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public Image? ProfileAvatar { get; set; }

        [Display(Name = nameof(ProfileAbout), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileAbout { get; set; }

        [Display(Name = nameof(ProfileGender), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;

        [Display(Name = nameof(ProfileGenderOwn), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileGenderOwn { get; set; }

        [Display(Name = nameof(FollowersCount), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int FollowersCount { get; set; }

        [Display(Name = nameof(FollowedCount), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int FollowedCount { get; set; }

        [Display(Name = nameof(PostsCount), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int PostsCount { get; set; }

        [Display(Name = nameof(Experience), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int Experience { get; set; }

        //References
        //Black list
        [Display(Name = nameof(BlockedProfiles), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<BlockedProfile>? BlockedProfiles { get; set; }

        [Display(Name = nameof(BlockedByProfiles), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<BlockedProfile>? BlockedByProfiles { get; set; }

        //Black list
        [Display(Name = nameof(Followers), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<Follower>? Followers { get; set; } //profile followers list

        [Display(Name = nameof(Followed), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<Follower>? Followed { get; set; } //profile followed profiles list

        //Chat
        [Display(Name = nameof(ChatMembers), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<ChatMember>? ChatMembers { get; set; } //List of ChatRooms, where profile participate

        [Display(Name = nameof(Messages), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<Message>? Messages { get; set; }

        [Display(Name = nameof(Favorites), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<Favorite>? Favorites { get; set; } //list of profile likes

        [Display(Name = nameof(Posts), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<Post>? Posts { get; set; } //List of profile posts

        [Display(Name = nameof(Comments), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<Comment>? Comments { get; set; } //List of profile comments

        [Display(Name = nameof(ProfileGifts), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<ProfileGift>? ProfileGifts { get; set; } //List of profile gifts

        [Display(Name = nameof(ProfileRanks), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ICollection<ProfileRank>? ProfileRanks { get; set; } //List of profile ranks

        [Display(Name = nameof(IsUserFollows), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public bool IsUserFollows { get; set; }

        [Display(Name = nameof(IsUserBlocks), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public bool IsUserBlocks { get; set; }

        [Display(Name = nameof(IsUserBlocked), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public bool IsUserBlocked { get; set; }
    }

    public class ProfileLimited
    {
        [Display(Name = nameof(UserName), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string UserName { get; set; } = default!;

        [Display(Name = nameof(LastLoginDateTime), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public DateTime? LastLoginDateTime { get; set; }

        [Display(Name = nameof(ProfileFullName), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string? ProfileFullName { get; set; }

        [Display(Name = nameof(ProfileWorkPlace), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string? ProfileWorkPlace { get; set; }

        [Display(Name = nameof(ProfileStatus), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string? ProfileStatus { get; set; }

        [Display(Name = nameof(ProfileAvatarId), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public Guid? ProfileAvatarId { get; set; }

        [Display(Name = nameof(ProfileAbout), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string? ProfileAbout { get; set; }

        [Display(Name = nameof(FollowersCount), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int FollowersCount { get; set; }

        [Display(Name = nameof(FollowedCount), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int FollowedCount { get; set; }

        [Display(Name = nameof(PostsCount), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int PostsCount { get; set; }

        [Display(Name = nameof(Experience), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int Experience { get; set; }

        [Display(Name = nameof(Rank), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public ProfileRank Rank { get; set; } = default!;
    }

    public class ProfileEdit : MUser
    {
        [Display(Name = nameof(ProfileFullName), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MinLength(1, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MinLength")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileFullName { get; set; }

        [Display(Name = nameof(ProfileWorkPlace), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileWorkPlace { get; set; }

        [Display(Name = nameof(ProfileStatus), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileStatus { get; set; }

        [Display(Name = nameof(ProfileAvatarId), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public Guid? ProfileAvatarId { get; set; }

        [Display(Name = nameof(ProfileAvatar), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public Image? ProfileAvatar { get; set; }

        [Display(Name = nameof(ProfileAbout), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileAbout { get; set; }

        [Display(Name = nameof(ProfileGender), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Range")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public ProfileGender ProfileGender { get; set; } = ProfileGender.Undefined;

        [Display(Name = nameof(ProfileGenderOwn), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? ProfileGenderOwn { get; set; }

        [Display(Name = nameof(Experience), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public int Experience { get; set; }

        [Display(Name = nameof(Password), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
        public string? Password { get; set; }
    }
}