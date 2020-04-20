using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;

namespace DAL.Repositories
{
    public class ProfileGiftRepo : BaseRepo<Domain.ProfileGift, ProfileGift, ApplicationDbContext>, IProfileGiftRepo
    {
        public ProfileGiftRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new BaseDALMapper<Domain.ProfileGift, ProfileGift>())
        {
        }
    }
}