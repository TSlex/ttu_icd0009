using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Microsoft.AspNetCore.Identity;
using Profile = BLL.App.DTO.Profile;

namespace BLL.App.Services
{
    public class ProfileService : BaseEntityService<IProfileRepo, DAL.App.DTO.Profile, Profile>, IProfileService
    {
        private readonly IAppUnitOfWork _uow;

        public ProfileService(IAppUnitOfWork uow) : base(uow.Profiles,
            new ProfileMapper())
        {
            _uow = uow;
        }
        
        [Obsolete]
        public async Task<Profile> GetProfileFull(Guid id)
        {
            var profile = Mapper.Map(await ServiceRepository.FindFullIncludeAsync(id));
            profile.PostsCount = profile.Posts?.Count ?? 0;
            profile.FollowersCount = profile.Followers?.Count ?? 0;
            profile.FollowedCount = profile.Followed?.Count ?? 0;

            return profile;
        }
        
        [Obsolete]
        public async Task<ProfileLimited> GetProfileLimited(Guid id)
        {
            var profile = await GetProfileFull(id);

            return new ProfileLimited()
            {
                UserName = profile.UserName,
                LastLoginDateTime = profile.LastLoginDateTime,
                ProfileAbout = profile.ProfileAbout,
                ProfileStatus = profile.ProfileStatus,
                ProfileAvatarId = profile.ProfileAvatarId,
                ProfileFullName = profile.ProfileFullName,
                ProfileWorkPlace = profile.ProfileWorkPlace,
                FollowedCount = profile.FollowedCount,
                FollowersCount = profile.FollowersCount,
                PostsCount = profile.PostsCount,
                Experience = profile.Experience,
                Rank = profile.ProfileRanks.OrderBy(rank => rank.Rank!.MaxExperience).ToList()[0]
            };
        }

        public async Task<Profile> GetProfileAsync(Guid id, Guid? requesterId)
        {
            var profile = await ServiceRepository.GetProfile(id, requesterId);

            if (profile.ProfileRanks.Count <= 0)
            {
                var rank = new DAL.App.DTO.ProfileRank()
                {
                    Id = Guid.NewGuid(),
                    RankId = (await _uow.Ranks.FindByCodeAsync("X_00")).Id
                };

                _uow.ProfileRanks.Add(rank);
                await _uow.SaveChangesAsync();

                profile.ProfileRanks = new[] {rank};
            }
            
            profile.PostsCount = profile.Posts?.Count ?? 0;
            profile.FollowersCount = profile.Followers?.Count ?? 0;
            profile.FollowedCount = profile.Followed?.Count ?? 0;

            profile.ProfileRanks = profile.ProfileRanks
                .OrderByDescending(rank => rank.Rank!.MaxExperience)
                .Where(rank => rank.Rank!.MinExperience <= profile.Experience)
                .Take(1).ToList();

            return Mapper.Map(profile);
        }

        public async Task<Profile> FindByUsernameAsync(string username)
        {
            return Mapper.Map(await ServiceRepository.FindByUsernameAsync(username));
        }
    }
}