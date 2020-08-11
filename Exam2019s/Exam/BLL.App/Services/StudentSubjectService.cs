using BLL.App.DTO;
using BLL.App.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.BLL.Base.Services;

 namespace BLL.App.Services
{
    public class StudentSubjectService : BaseEntityService<IStudentSubjectRepo, DAL.App.DTO.StudentSubject, StudentSubject>, IStudentSubjectService
    {
        public StudentSubjectService(IAppUnitOfWork uow) :
            base(uow.StudentSubjects, new UniversalBLLMapper<DAL.App.DTO.StudentSubject, StudentSubject>())
        {
        }
    }
}