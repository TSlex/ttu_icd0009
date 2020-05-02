using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using ProfileRank = DAL.App.DTO.ProfileRank;
using Rank = BLL.App.DTO.Rank;

namespace BLL.App.Services
{
    public class RankService : BaseEntityService<IRankRepo, DAL.App.DTO.Rank, Rank>, IRankService
    {
        private readonly IAppUnitOfWork _uow;
        
        public RankService(IAppUnitOfWork uow) :
            base(uow.Ranks, new RankMapper())
        {
            _uow = uow;
        }

        public async Task<Rank> FindByCodeAsync(string code)
        {
            return Mapper.Map(await ServiceRepository.FindByCodeAsync(code));
        }

        public async Task IncreaseUserExperience(Guid userId, int amount)
        {
            var profile = await _uow.Profiles.FindFullIncludeAsync(userId);
            
            //add profile next rank if possible
            var profileRank = profile.ProfileRanks.OrderByDescending(rank => rank.Rank!.MaxExperience).Take(1).ToList()[0].Rank;

            if (profile.Experience + amount > profileRank!.MaxExperience && profileRank.NextRank != null)
            {
                _uow.ProfileRanks.Add(new ProfileRank()
                {
                    ProfileId = userId,
                    RankId = profileRank.NextRank.Id
                });
            }
            
            //increase profile experience
            if (profile.Experience + amount <= profileRank.MaxExperience || profileRank.NextRank != null)
            {
                await _uow.Profiles.IncreaseExperience(userId, amount);
            }
            else if (profile.Experience <= profileRank.MaxExperience && profile.Experience + amount > profileRank.MaxExperience && profileRank.NextRank == null)
            {
                await _uow.Profiles.IncreaseExperience(userId, profileRank.MaxExperience - profile.Experience);
            }

            await _uow.SaveChangesAsync();
        }
    }
}