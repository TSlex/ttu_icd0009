using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class BlockedProfileService : BaseEntityService<IBlockedProfileRepo, DAL.App.DTO.BlockedProfile, BlockedProfile>, IBlockedProfileService
    {
        public BlockedProfileService(IAppUnitOfWork uow) :
            base(uow.BlockedProfiles, new BlockedProfileMapper())
        {
        }

        public BlockedProfile AddBlockProperty(Guid userId, Guid profileId)
        {
            return Mapper.Map(ServiceRepository.Add(new DAL.App.DTO.BlockedProfile()
            {
                ProfileId = userId,
                BProfileId = profileId
            }));
        }

        public async Task<BlockedProfile> RemoveBlockPropertyAsync(Guid userId, Guid profileId)
        {
            var blockedProperty = await ServiceRepository.FindAsync(userId, profileId);

            return blockedProperty != null ? Mapper.Map(ServiceRepository.Remove(blockedProperty)) : null;
        }

        public async Task<BlockedProfile> FindAsync(Guid userId, Guid profileId)
        {
            return Mapper.Map(await ServiceRepository.FindAsync(userId, profileId));
        }

        public async Task<int> CountByIdAsync(Guid userId)
        {
            return await ServiceRepository.CountByIdAsync(userId);
        }

        public async Task<IEnumerable<BlockedProfile>> AllByIdPageAsync(Guid userId, int pageNumber, int count)
        {
            return (await ServiceRepository.AllByIdPageAsync(userId, pageNumber, count)).Select(profile => Mapper.Map(profile));
        }
    }
}