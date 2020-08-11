using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF;
using DAL.App.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;

namespace DAL.App.Repositories
{
    public class HomeWorkRepo : BaseRepo<Domain.HomeWork, HomeWork, ApplicationDbContext>,
        IHomeWorkRepo
    {
        public HomeWorkRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.HomeWork, HomeWork>())
        {
        }
    }
}