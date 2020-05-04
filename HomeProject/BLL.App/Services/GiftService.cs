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
    public class GiftService : BaseEntityService<IGiftRepo, DAL.App.DTO.Gift, Gift>, IGiftService
    {
        public GiftService(IAppUnitOfWork uow) :
            base(uow.Gifts, new GiftMapper())
        {
        }

        public async Task<Gift> FindByCodeAsync(string giftCode)
        {
            return Mapper.Map(await ServiceRepository.FindByCodeAsync(giftCode));
        }

        public async Task<IEnumerable<Gift>> Get10ByPageAsync(int pageNumber)
        {
            var result = await ServiceRepository.Get10ByPageAsync(pageNumber, 10);
            return result.Select(gift => Mapper.Map(gift));
        }

        public async Task<GiftsCount> GetCountAsync()
        {
            return new GiftsCount()
            {
                Count = await ServiceRepository.GetCountAsync()
            };
        }
    }
}