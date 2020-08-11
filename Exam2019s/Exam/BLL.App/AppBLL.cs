using BLL.App.Services;
using ee.itcollege.aleksi.BLL.Base;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;


namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        public AppBLL(IAppUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IHomeWorkService HomeWorks => GetService<IHomeWorkService>(() => new HomeWorkService(UnitOfWork));
        public ISemesterService Semesters => GetService<ISemesterService>(() => new SemesterService(UnitOfWork));
        public ISubjectService Subjects => GetService<ISubjectService>(() => new SubjectService(UnitOfWork));
        public IStudentSubjectService StudentSubjects => GetService<IStudentSubjectService>(() => new StudentSubjectService(UnitOfWork));
        public IStudentHomeWorkService StudentHomeWorks => GetService<IStudentHomeWorkService>(() => new StudentHomeWorkService(UnitOfWork));
    }
}