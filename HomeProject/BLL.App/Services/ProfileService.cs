using System;
using System.Threading.Tasks;
using AutoMapper;
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

        public ProfileService(IAppUnitOfWork uow) : base(uow.Profiles,
            new ProfileMapper())
        {
        }

        public async Task<Profile> GetProfileFull(Guid id)
        {
            var profile = Mapper.Map(await ServiceRepository.FindAsync(id));
            profile.PostsCount = profile.Posts?.Count ?? 0;
            profile.FollowersCount = profile.Followers?.Count ?? 0;
            profile.FollowedCount = profile.Followed?.Count ?? 0;

            return profile;
        }

        public async Task<Profile> FindByUsernameAsync(string username)
        {
            return Mapper.Map(await ServiceRepository.FindByUsernameAsync(username));
        }
    }
}