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
    public class ProfileRankService : BaseEntityService<IProfileRankRepo, DAL.App.DTO.ProfileRank, ProfileRank>, IProfileRankService
    {
        public ProfileRankService(IAppUnitOfWork uow) :
            base(uow.ProfileRanks, new UniversalBLLMapper<DAL.App.DTO.ProfileRank, ProfileRank>())
        {
            
        }

        public async Task<IEnumerable<ProfileRank>> AllUserAsync(Guid profileId)
        {
            return (await ServiceRepository.AllUserAsync(profileId)).Select(rank => Mapper.Map(rank));
        }
    }
}