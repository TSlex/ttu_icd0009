using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class RankService : BaseEntityService<IRankRepo, DAL.App.DTO.Rank, Rank>, IRankService
    {
        public RankService(IAppUnitOfWork uow) :
            base(uow.Ranks, new RankMapper())
        {
        }
    }
}