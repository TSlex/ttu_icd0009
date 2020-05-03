using BLL.App.DTO;
using BLL.Base.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class ImageService : BaseEntityService<IImageRepo, DAL.App.DTO.Image, Image>, IImageService
    {
        public ImageService(IAppUnitOfWork uow) :
            base(uow.Images, new BaseBLLMapper<DAL.App.DTO.Image, Image>())
        {
        }
    }
}