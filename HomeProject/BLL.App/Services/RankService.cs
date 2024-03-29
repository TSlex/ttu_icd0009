﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Mappers;
using ee.itcollege.aleksi.BLL.Base.Services;
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
            base(uow.Ranks, new UniversalBLLMapper<DAL.App.DTO.Rank, Rank>())
        {
            _uow = uow;
        }

        public async Task<bool> NextRankExists(Guid id, Guid? reqGuid)
        {
            return await ServiceRepository.NextRankExists(id, reqGuid);
        }

        public async Task<bool> PreviousRankExists(Guid id, Guid? reqGuid)
        {
            return await ServiceRepository.PreviousRankExists(id, reqGuid);
        }

        public async Task<Rank> FindByCodeAsync(string code)
        {
            return Mapper.Map(await ServiceRepository.FindByCodeAsync(code));
        }

        public async Task IncreaseUserExperience(Guid userId, int amount)
        {
            var profile = await _uow.Profiles.FindRankIncludeAsync(userId);

            //add profile next rank if possible
            var profileRank =
                profile.ProfileRanks.OrderByDescending(rank => rank.Rank!.MaxExperience).Take(1).ToList()[0].Rank;

            if (profile.Experience + amount >= profileRank!.MaxExperience && profileRank.NextRankId != null)
            {
                _uow.ProfileRanks.Add(new ProfileRank()
                {
                    ProfileId = userId,
                    RankId = (Guid) profileRank.NextRankId
                });
            }

            //increase profile experience
            if (profile.Experience + amount <= profileRank.MaxExperience || profileRank.NextRank != null)
            {
                await _uow.Profiles.IncreaseExperience(userId, amount);
            }
            else if (profile.Experience <= profileRank.MaxExperience &&
                     profile.Experience + amount > profileRank.MaxExperience && profileRank.NextRank == null)
            {
                await _uow.Profiles.IncreaseExperience(userId, profileRank.MaxExperience - profile.Experience);
            }

            await _uow.SaveChangesAsync();
        }
    }
}