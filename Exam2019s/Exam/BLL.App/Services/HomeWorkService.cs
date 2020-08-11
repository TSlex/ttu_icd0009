using BLL.App.DTO;
using BLL.App.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.BLL.Base.Services;

 namespace BLL.App.Services
{
    public class HomeWorkService : BaseEntityService<IHomeWorkRepo, DAL.App.DTO.HomeWork, HomeWork>, IHomeWorkService
    {
        public HomeWorkService(IAppUnitOfWork uow) :
            base(uow.HomeWorks, new UniversalBLLMapper<DAL.App.DTO.HomeWork, HomeWork>())
        {
        }
    }
}