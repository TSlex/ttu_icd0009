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
    }
}