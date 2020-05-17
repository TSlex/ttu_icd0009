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

namespace BLL.App.Services
{
    public class ProfileService : BaseEntityService<IProfileRepo, DAL.App.DTO.Profile, ProfileFull>, IProfileService
    {

        public ProfileService(IAppUnitOfWork uow) : base(uow.Profiles,
            new ProfileMapper())
        {
        }

        public async Task<ProfileFull> GetProfileFull(Guid id)
        {
            var profile = Mapper.Map(await ServiceRepository.FindFullIncludeAsync(id));
            profile.PostsCount = profile.Posts?.Count ?? 0;
            profile.FollowersCount = profile.Followers?.Count ?? 0;
            profile.FollowedCount = profile.Followed?.Count ?? 0;

            return profile;
        }

        public async Task<ProfileLimited> GetProfileLimited(Guid id)
        {
            var profile = await GetProfileFull(id);

            return new ProfileLimited()
            {
                UserName = profile.UserName,
                LastLoginDateTime = profile.LastLoginDateTime,
                ProfileAbout = profile.ProfileAbout,
                ProfileStatus = profile.ProfileStatus,
//                ProfileAvatarUrl = profile.ProfileAvatarUrl,
                ProfileFullName = profile.ProfileFullName,
                ProfileWorkPlace = profile.ProfileWorkPlace,
                FollowedCount = profile.FollowedCount,
                FollowersCount = profile.FollowersCount,
                PostsCount = profile.PostsCount,
                Experience = profile.Experience,
                Rank = profile.ProfileRanks.OrderBy(rank => rank.Rank!.MaxExperience).ToList()[0]
            };
        }

        public async Task<ProfileFull> FindByUsernameAsync(string username)
        {
            return Mapper.Map(await ServiceRepository.FindByUsernameAsync(username));
        }
    }
}