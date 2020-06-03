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

            return Mapper.Map(profile);
        }

        public async Task<Profile> FindByUsernameAsync(string username)
        {
            return Mapper.Map(await ServiceRepository.FindByUsernameAsync(username));
        }

        public async Task<Profile> FindByUsernameAsync(string username, Guid? requesterId)
        {
            return Mapper.Map(await ServiceRepository.FindByUsernameAsync(username, requesterId));
        }
        
        public async Task<Profile> FindByUsernameWithFollowersAsync(string username)
        {
            return Mapper.Map(await ServiceRepository.FindByUsernameWithFollowersAsync(username));
        }
    }
}