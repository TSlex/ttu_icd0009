using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using DAL.Helpers;
using DAL.Mappers;
using Domain.Translation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Follower = DAL.App.DTO.Follower;
using Gift = DAL.App.DTO.Gift;
using Profile = DAL.App.DTO.Profile;
using ProfileGift = DAL.App.DTO.ProfileGift;
using ProfileRank = DAL.App.DTO.ProfileRank;
using Rank = DAL.App.DTO.Rank;

namespace DAL.Repositories
{
    public class ProfileRepo : BaseRepo<Domain.Profile, Profile, ApplicationDbContext>, IProfileRepo
    {
        private readonly UserManager<Domain.Profile> _userManager;

        private readonly FollowerMapper _followerMapper;
        private readonly BlockedProfileMapper _blockedProfileMapper;
        private readonly PostMapper _postMapper;

        public ProfileRepo(ApplicationDbContext dbContext, UserManager<Domain.Profile> userManager)
            : base(dbContext, new ProfileMapper())
        {
            _userManager = userManager;
            _followerMapper = new FollowerMapper();
            _blockedProfileMapper = new BlockedProfileMapper();
            _postMapper = new PostMapper();
        }

        public async Task<Tuple<ProfileEdit, string[]>> UpdateProfileAdminAsync(ProfileEdit entity)
        {
            var record = await _userManager.FindByIdAsync(entity.Id.ToString());
            var errors = new List<string>();

            if (record == null)
            {
                errors.Add(Resourses.BLL.App.DTO.Common.ErrorUserNotFound);
                return new Tuple<ProfileEdit, string[]>(entity, errors.ToArray());
            }

            if (record.UserName != entity.UserName)
            {
                var result = await _userManager.SetUserNameAsync(record, entity.UserName);
                if (!result.Succeeded)
                {
                    errors.Add(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorChangeUsername);
                    return new Tuple<ProfileEdit, string[]>(entity, errors.ToArray());
                }
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(entity.Email);

            if (userWithSameEmail != null && userWithSameEmail.Id != entity.Id)
            {
                errors.Add(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorChangeEmail);
                return new Tuple<ProfileEdit, string[]>(entity, errors.ToArray());
            }
            else if (userWithSameEmail == null)
            {
                var result = await _userManager.SetEmailAsync(record, entity.Email);
                if (!result.Succeeded)
                {
                    errors.Add(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorChangeEmail);
                    return new Tuple<ProfileEdit, string[]>(entity, errors.ToArray());
                }
            }

            if (entity.Password != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(record);
                var changePasswordResult = await _userManager.ResetPasswordAsync(record, resetToken, entity.Password);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        errors.Add(error.Description);
                    }

                    return new Tuple<ProfileEdit, string[]>(entity, errors.ToArray());
                }
            }

            record.ProfileFullName = entity.ProfileFullName;
            record.ProfileWorkPlace = entity.ProfileWorkPlace;
            record.Experience = entity.Experience;
            record.ProfileAbout = entity.ProfileAbout;
            record.ProfileGender = entity.ProfileGender;
            record.ProfileGenderOwn = entity.ProfileGenderOwn;
            record.ProfileStatus = entity.ProfileStatus;
            record.PhoneNumber = entity.PhoneNumber;
            record.PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
            record.LockoutEnabled = entity.LockoutEnabled;
            record.EmailConfirmed = entity.EmailConfirmed;
            record.AccessFailedCount = entity.AccessFailedCount;
            record.ProfileAvatarId = entity.ProfileAvatarId;

            await _userManager.UpdateAsync(record);

            return new Tuple<ProfileEdit, string[]>(entity, errors.ToArray());
        }

        public async Task<bool> ExistsAsync(string username)
        {
            return await _userManager.FindByNameAsync(username) != null;
        }

        public async Task<Profile> GetProfile(Guid id, Guid? requesterId)
        {
            return await RepoDbSet
                .Where(profile => profile.Id == id && profile.DeletedAt == null)
                .Select(profile => new Profile()
                {
                    Id = profile.Id,
                    ProfileAbout = profile.ProfileAbout,
                    ProfileFullName = profile.ProfileFullName,
                    ProfileWorkPlace = profile.ProfileWorkPlace,
                    ProfileAvatarId = profile.ProfileAvatarId,
                    UserName = profile.UserName,
                    Experience = profile.Experience,
                    Followed = profile.Followed
                        .Select(follower => _followerMapper.Map(follower))
                        .ToList(),
                    Followers = profile.Followers
                        .Select(follower => _followerMapper.Map(follower))
                        .ToList(),
                    BlockedProfiles = profile.BlockedByProfiles
                        .Select(blockedProfile => _blockedProfileMapper.Map(blockedProfile))
                        .ToList(),
                    Posts = profile.Posts
                        .Where(post => post.DeletedAt == null)
                        .Select(post => _postMapper.Map(post))
                        .ToList(),
                    ProfileGifts = profile.ProfileGifts
                        .Where(gift => gift.DeletedAt == null)
                        .Select(gift => new ProfileGift()
                        {
                            Id = gift.Id,
                            GiftDateTime = gift.GiftDateTime,
                            Gift = new Gift()
                            {
                                Id = gift.Gift!.Id,
                                GiftImageId = gift.Gift.GiftImageId,
                                GiftName = new LangString()
                                    {
                                        Id = gift.Gift.GiftName!.Id,
                                        Translations = gift.Gift.GiftName!.Translations,
                                    }
                                    .ToString(),
                            }
                        })
                        .OrderByDescending(gift => gift.GiftDateTime)
                        .Take(5)
                        .ToList(),
                    ProfileRanks = profile.ProfileRanks
                        .Where(rank => rank.DeletedAt == null && rank.Rank!.MinExperience <= profile.Experience)
                        .Select(rank => new ProfileRank()
                        {
                            Id = rank.Id,
                            Rank = new Rank()
                            {
                                Id = rank.Rank!.Id,
                                MaxExperience = rank.Rank!.MaxExperience,
                                MinExperience = rank.Rank!.MinExperience,
                                RankCode = rank.Rank!.RankCode,
                                RankColor = rank.Rank!.RankColor,
                                RankDescription = new LangString()
                                    {
                                        Id = rank.Rank.RankDescription!.Id,
                                        Translations = rank.Rank.RankDescription!.Translations,
                                    }
                                    .ToString(),
                                RankIcon = rank.Rank.RankIcon,
                                RankTitle = new LangString()
                                    {
                                        Id = rank.Rank.RankTitle!.Id,
                                        Translations = rank.Rank.RankTitle!.Translations,
                                    }
                                    .ToString(),
                                RankTextColor = rank.Rank.RankTextColor
                            }
                        })
                        .OrderByDescending(rank => rank.Rank!.MaxExperience)
                        .Take(1)
                        .ToList(),
                    IsUserBlocked = requesterId != null && RepoDbContext.BlockedProfiles
                                        .Any(blockedProfile => blockedProfile.ProfileId == profile.Id
                                                               && blockedProfile.BProfileId == (Guid) requesterId),
                    IsUserBlocks = requesterId != null && RepoDbContext.BlockedProfiles
                                       .Any(blockedProfile => blockedProfile.BProfileId == profile.Id
                                                              && blockedProfile.ProfileId == (Guid) requesterId),
                    IsUserFollows = requesterId != null && RepoDbContext.Followers
                                        .Any(follower => follower.ProfileId == profile.Id
                                                         && follower.FollowerProfileId == (Guid) requesterId),
                }).FirstOrDefaultAsync();
        }

        public async Task<Profile> FindRankIncludeAsync(Guid id)
        {
            return Mapper.Map(await _userManager.Users
                .Include(profile => profile.ProfileRanks)
                .ThenInclude(rank => rank.Rank)
                .ThenInclude(rank => rank!.RankTitle)
                .ThenInclude(title => title!.Translations)
                .Include(profile => profile.ProfileRanks)
                .ThenInclude(rank => rank.Rank)
                .ThenInclude(rank => rank!.RankDescription)
                .ThenInclude(desc => desc!.Translations)
                .FirstOrDefaultAsync(profile => profile.Id == id));
        }

        public async Task<Profile> FindByUsernameAsync(string username)
        {
            return Mapper.Map(await RepoDbSet.FirstOrDefaultAsync(profile => profile.UserName == username));
        }

        public async Task<Profile> FindByUsernameWithFollowersAsync(string username)
        {
            return await RepoDbSet
                .Where(profile => profile.UserName == username)
                .Select(profile => new Profile()
                {
                    Followed = profile.Followed.Select(follower => new Follower()
                    {
                        Id = follower.Id,
                        Profile = new Profile()
                        {
                            UserName = follower.Profile!.UserName,
                            ProfileAvatarId = follower.Profile!.ProfileAvatarId,
                        }
                    }).ToList(),
                    Followers = profile.Followers.Select(follower => new Follower()
                    {
                        Id = follower.Id,
                        FollowerProfile = new Profile()
                        {
                            UserName = follower.FollowerProfile!.UserName,
                            ProfileAvatarId = follower.FollowerProfile!.ProfileAvatarId,
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Profile> FindByUsernameAsync(string username, Guid? requesterId)
        {
            return await RepoDbSet.Where(profile => profile.UserName == username
                                                    && profile.DeletedAt == null)
                .Select(profile => new Profile()
                {
                    Id = profile.Id,
                    ProfileAbout = profile.ProfileAbout,
                    ProfileFullName = profile.ProfileFullName,
                    ProfileWorkPlace = profile.ProfileWorkPlace,
                    ProfileAvatarId = profile.ProfileAvatarId,
                    UserName = profile.UserName,
                    Experience = profile.Experience,
                    Followed = profile.Followed
                        .Select(follower => _followerMapper.Map(follower))
                        .ToList(),
                    Followers = profile.Followers
                        .Select(follower => _followerMapper.Map(follower))
                        .ToList(),
                    FollowedCount = profile.Followed!.Count,
                    FollowersCount = profile.Followers!.Count,
                    PostsCount = profile.Posts.Count(post => post.DeletedAt == null && post.MasterId == null),
                    IsUserBlocked = requesterId != null && RepoDbContext.BlockedProfiles
                                        .Any(blockedProfile => blockedProfile.ProfileId == profile.Id
                                                               && blockedProfile.BProfileId == (Guid) requesterId),
                    IsUserBlocks = requesterId != null && RepoDbContext.BlockedProfiles
                                       .Any(blockedProfile => blockedProfile.BProfileId == profile.Id
                                                              && blockedProfile.ProfileId == (Guid) requesterId),
                    IsUserFollows = requesterId != null && RepoDbContext.Followers
                                        .Any(follower => follower.ProfileId == profile.Id
                                                         && follower.FollowerProfileId == (Guid) requesterId),
                })
                .FirstOrDefaultAsync();
        }

        public async Task IncreaseExperience(Guid userId, int amount)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return;
            }

            user.Experience += amount;

            await _userManager.UpdateAsync(user);
        }

        public override Task<Profile> UpdateAsync(Profile entity)
        {
            throw new NotSupportedException();
        }

        public override void Restore(Profile tEntity)
        {
            var entity = _userManager.FindByIdAsync(tEntity.Id.ToString()).Result;

            RepoDbContext.Entry(entity).State = EntityState.Modified;

            entity.DeletedAt = null;
            entity.DeletedBy = null;

            RepoDbContext.SaveChanges();
        }

        public override Profile Remove(Profile tEntity)
        {
            var entity = _userManager.FindByIdAsync(tEntity.Id.ToString()).Result;

            var blockedProfiles =
                RepoDbContext.BlockedProfiles.Where(profile => profile.ProfileId == entity.Id).ToList();

            foreach (var blockedProfile in blockedProfiles)
            {
                RepoDbContext.BlockedProfiles.Remove(blockedProfile);
            }

            var followers = RepoDbContext.Followers.Where(follower =>
                follower.FollowerProfileId == entity.Id || follower.ProfileId == entity.Id);

            foreach (var follower in followers)
            {
                RepoDbContext.Followers.Remove(follower);
            }

            var members = RepoDbContext.ChatMembers.Where(member => member.ProfileId == entity.Id).ToList();

            foreach (var chatMember in members)
            {
                RepoDbContext.ChatMembers.Remove(chatMember);
            }

            var messages = RepoDbContext.Messages.Where(message => message.ProfileId == entity.Id).ToList();

            foreach (var message in messages)
            {
                RepoDbContext.Messages.Remove(message);
            }

            var favorites = RepoDbContext.Favorites.Where(favorite => favorite.ProfileId == entity.Id).ToList();

            foreach (var favorite in favorites)
            {
                RepoDbContext.Favorites.Remove(favorite);
            }

            var posts = RepoDbContext.Posts.Where(post => post.ProfileId == entity.Id).ToList();

            foreach (var post in posts)
            {
                RepoDbContext.Posts.Remove(post);
            }

            var comments = RepoDbContext.Comments.Where(comment => comment.ProfileId == entity.Id).ToList();

            foreach (var comment in comments)
            {
                RepoDbContext.Comments.Remove(comment);
            }

//            var ranks = RepoDbContext.ProfileRanks.Where(rank => rank.ProfileId == entity.Id).ToList();
//
//            foreach (var rank in ranks)
//            {
//                RepoDbContext.ProfileRanks.Remove(rank);
//            }

            var profileGifts = RepoDbContext.ProfileGifts.Where(gift => gift.ProfileId == entity.Id);

            foreach (var profileGift in profileGifts)
            {
                RepoDbContext.ProfileGifts.Remove(profileGift);
            }

//            var imageRecord = RepoDbContext.Images.FirstOrDefault(image => image.Id == entity.ProfileAvatarId);
//
//            if (imageRecord != null)
//            {
//                RepoDbContext.Images.Remove(imageRecord);
//            }

            RepoDbContext.Entry(entity).State = EntityState.Modified;

            entity.DeletedAt = DateTime.Now;
            entity.DeletedBy = "system";

            RepoDbContext.SaveChanges();

            return tEntity;

//            return base.Remove(_entity);
        }
    }
}