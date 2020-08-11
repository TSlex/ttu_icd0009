using BLL.App.DTO;
using BLL.App.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.BLL.Base.Services;

 namespace BLL.App.Services
{
    public class StudentHomeWorkService : BaseEntityService<IStudentHomeWorkRepo, DAL.App.DTO.StudentHomeWork, StudentHomeWork>, IStudentHomeWorkService
    {
        public StudentHomeWorkService(IAppUnitOfWork uow) :
            base(uow.StudentHomeWorks, new UniversalBLLMapper<DAL.App.DTO.StudentHomeWork, StudentHomeWork>())
        {
        }
    }
}