using DAL.App.DTO;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Mappers;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;

namespace DAL.Repositories
{
    public class ImageRepo : BaseRepo<Domain.Image, Image, ApplicationDbContext>, IImageRepo
    {
        public ImageRepo(ApplicationDbContext dbContext) :
            base(dbContext, new BaseDALMapper<Domain.Image, Image>())
        {
        }
    }
}