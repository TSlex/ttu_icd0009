using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class ProfileGiftService : BaseEntityService<IProfileGiftRepo, DAL.App.DTO.ProfileGift, ProfileGift>, IProfileGiftService
    {
        public ProfileGiftService(IAppUnitOfWork uow) :
            base(uow.ProfileGifts, new ProfileGiftMapper())
        {
        }
    }
}