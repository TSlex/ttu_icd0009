using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class GiftService : BaseEntityService<IGiftRepo, DAL.App.DTO.Gift, Gift>, IGiftService
    {
        public GiftService(IAppUnitOfWork uow) :
            base(uow.Gifts, new GiftMapper())
        {
        }
    }
}