using BLL.App.DTO;
using BLL.App.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.BLL.Base.Services;

 namespace BLL.App.Services
{
    public class SubjectService : BaseEntityService<ISubjectRepo, DAL.App.DTO.Subject, Subject>, ISubjectService
    {
        public SubjectService(IAppUnitOfWork uow) :
            base(uow.Subjects, new UniversalBLLMapper<DAL.App.DTO.Subject, Subject>())
        {
        }
    }
}