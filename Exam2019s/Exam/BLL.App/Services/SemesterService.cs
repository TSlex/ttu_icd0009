using BLL.App.DTO;
using BLL.App.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.BLL.Base.Services;

 namespace BLL.App.Services
{
    public class SemesterService : BaseEntityService<ISemesterRepo, DAL.App.DTO.Semester, Semester>, ISemesterService
    {
        public SemesterService(IAppUnitOfWork uow) :
            base(uow.Semesters, new UniversalBLLMapper<DAL.App.DTO.Semester, Semester>())
        {
        }
    }
}