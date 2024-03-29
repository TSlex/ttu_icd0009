﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using ee.itcollege.aleksi.BLL.Base.Services;
using Contracts.BLL.App.Services;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class FollowerService : BaseEntityService<IFollowerRepo, DAL.App.DTO.Follower, Follower>, IFollowerService
    {
        public FollowerService(IAppUnitOfWork uow) :
            base(uow.Followers, new UniversalBLLMapper<DAL.App.DTO.Follower, Follower>())
        {
        }

        public Follower AddSubscription(Guid userId, Guid profileId)
        {
            return Mapper.Map(ServiceRepository.Add(new DAL.App.DTO.Follower()
            {
                FollowerProfileId = userId,
                ProfileId = profileId
            }));
        }

        public async Task<Follower?> RemoveSubscriptionAsync(Guid userId, Guid profileId)
        {
            var subscription = await ServiceRepository.FindAsync(userId, profileId);

            if (subscription != null)
            {
                return Mapper.Map(ServiceRepository.Remove(subscription));
            }

            return null;
        }
        
        public async Task<Follower> FindAsync(Guid userId, Guid profileId)
        {
            return Mapper.Map(await ServiceRepository.FindAsync(userId, profileId));
        }

        public async Task<int> CountByIdAsync(Guid userId, bool reversed)
        {
            return await ServiceRepository.CountByIdAsync(userId, reversed);
        }

        public async Task<IEnumerable<Follower>> AllByIdPageAsync(Guid userId, bool reversed, int pageNumber, int count)
        {
            return (await ServiceRepository.AllByIdPageAsync(userId, reversed, pageNumber, count)).Select(follower => Mapper.Map(follower));
        }
    }
}