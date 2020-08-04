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
    public class ProfileGiftService : BaseEntityService<IProfileGiftRepo, DAL.App.DTO.ProfileGift, ProfileGift>, IProfileGiftService
    {
        public ProfileGiftService(IAppUnitOfWork uow) :
            base(uow.ProfileGifts, new UniversalBLLMapper<DAL.App.DTO.ProfileGift, ProfileGift>())
        {
        }

        public async Task<IEnumerable<ProfileGift>> GetUser10ByPageAsync(Guid userId, int pageNumber)
        {
            var result = await ServiceRepository.GetByPageAsync(userId, pageNumber, 10);
            return result.Select(gift => Mapper.Map(gift));
        }

        public async Task<GiftsCount> GetUserCountAsync(Guid userId)
        {
            return new GiftsCount()
            {
                Count = await ServiceRepository.GetUserCountAsync(userId)
            };
        }
    }
}