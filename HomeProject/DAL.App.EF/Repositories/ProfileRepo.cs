﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.DTO;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using DAL.Helpers;
using DAL.Mappers;
using Domain.Translation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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

        public async Task<Profile> GetProfile(Guid id, Guid? requesterId)
        {
            return await RepoDbSet.Where(profile => profile.Id == id)
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
                        .Select(post => _postMapper.Map(post))
                        .ToList(),
                    ProfileGifts = profile.ProfileGifts
                        .Select(gift => new ProfileGift()
                        {
                            Id = gift.Id,
                            Gift = new Gift()
                            {
                                Id = gift.Gift.Id,
                                GiftImageId = gift.Gift.GiftImageId,
                                GiftName = new LangString()
                                    {
                                        Id = gift.Gift.GiftName.Id,
                                        Translations = gift.Gift.GiftName.Translations,
                                    }
                                    .ToString(),
                            }
                        })
                        .ToList(),
                    ProfileRanks = profile.ProfileRanks
                        .Select(rank => new ProfileRank()
                        {
                            Id = rank.Id,
                            Rank = new Rank()
                            {
                                Id = rank.Rank.Id,
                                MaxExperience = rank.Rank.MaxExperience,
                                MinExperience = rank.Rank.MinExperience,
                                RankCode = rank.Rank.RankCode,
                                RankColor = rank.Rank.RankColor,
                                RankDescription = new LangString()
                                    {
                                        Id = rank.Rank.RankDescription.Id,
                                        Translations = rank.Rank.RankDescription.Translations,
                                    }
                                    .ToString(),
                                RankIcon = rank.Rank.RankIcon,
                                RankTitle = new LangString()
                                    {
                                        Id = rank.Rank.RankTitle.Id,
                                        Translations = rank.Rank.RankTitle.Translations,
                                    }
                                    .ToString(),
                                RankTextColor = rank.Rank.RankTextColor
                            }
                        })
                        .ToList(),
                    IsUserBlocked = requesterId != null && RepoDbContext.BlockedProfiles
                                        .Any(blockedProfile => blockedProfile.ProfileId == profile.Id
                                                               && blockedProfile.BProfileId == (Guid) requesterId),
                    IsUserBlocks = requesterId != null && RepoDbContext.BlockedProfiles
                                       .Any(blockedProfile => blockedProfile.BProfileId == profile.Id
                                                              && blockedProfile.ProfileId == (Guid) requesterId),
                    IsUserFollows = requesterId != null && RepoDbContext.Followers
                                        .Any(follower => follower.ProfileId == (Guid) requesterId
                                                         && follower.FollowerProfileId == profile.Id),
                }).FirstOrDefaultAsync();
        }

        public async Task<Profile> FindFullIncludeAsync(Guid id)
        {
            return Mapper.Map(await _userManager.Users
                .Include(profile => profile.Posts)
                .Include(profile => profile.Followed)
                .Include(profile => profile.Followers)
                .Include(profile => profile.ProfileGifts)
                .ThenInclude(gift => gift.Gift)
                .Include(profile => profile.ProfileRanks)
                .ThenInclude(rank => rank.Rank)
                .ThenInclude(rank => rank!.RankTitle)
                .ThenInclude(title => title!.Translations)
                .Include(profile => profile.ProfileRanks)
                .ThenInclude(rank => rank.Rank)
                .ThenInclude(rank => rank!.RankDescription)
                .ThenInclude(desc => desc!.Translations)
//                .Include(profile => profile.ProfileRanks)
//                .ThenInclude(rank => rank.Rank)
//                .ThenInclude(rank => rank!.NextRank)
                .FirstOrDefaultAsync(profile => profile.Id == id));
        }

        public async Task<Profile> FindNoIncludeAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet
                .Include(profile => profile.Posts!.Count)
                .Include(profile => profile.Followed!.Count)
                .Include(profile => profile.Followers!.Count)
                .FirstOrDefaultAsync(profile => profile.Id == id));
        }

#pragma warning disable 8604
        public async Task<Profile> FindByUsernameAsync(string username)
        {
            var result = await _userManager.Users
                .Where(profile => profile.UserName == username)
                .Select(profile => new
                {
                    value = profile,
                    postsCount = profile.Posts!.Count,
                    followedCount = profile.Followed!.Count,
                    followersCount = profile.Followers!.Count
                }).FirstOrDefaultAsync();

            if (result!.value == null) return Mapper.Map(result!.value);

            result.value.PostsCount = result.postsCount;
            result.value.FollowedCount = result.followedCount;
            result.value.FollowersCount = result.followersCount;

            return Mapper.Map(result.value);
        }
#pragma warning restore 8604

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
            throw new NotImplementedException();

            /*var stamp = (await _userManager.FindByIdAsync(entity.Id.ToString())).SecurityStamp;
            var mappedEntity = Mapper.MapReverse(entity);
            
            mappedEntity.SecurityStamp = stamp;
            
            var result = await _userManager.UpdateAsync(mappedEntity);

            return result.Succeeded ? entity : null;*/
        }
    }
}