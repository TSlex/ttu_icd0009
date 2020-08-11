using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.EF;
using DAL.App.Repositories;
using ee.itcollege.aleksi.DAL.Base.EF;

namespace DAL.App
{
    public class AppUnitOfWork : EFBaseUnitOfWork<ApplicationDbContext>, IAppUnitOfWork
    {
        public AppUnitOfWork(ApplicationDbContext uowDbContext) : base(uowDbContext)
        {
        }

        public IHomeWorkRepo HomeWorks => GetRepository<IHomeWorkRepo>(() => new HomeWorkRepo(UOWDbContext));
        public ISemesterRepo Semesters => GetRepository<ISemesterRepo>(() => new SemesterRepo(UOWDbContext));
        public ISubjectRepo Subjects => GetRepository<ISubjectRepo>(() => new SubjectRepo(UOWDbContext));
        public IStudentSubjectRepo StudentSubjects => GetRepository<IStudentSubjectRepo>(() => new StudentSubjectRepo(UOWDbContext));
        public IStudentHomeWorkRepo StudentHomeWorks => GetRepository<IStudentHomeWorkRepo>(() => new StudentHomeWorkRepo(UOWDbContext));
    }
}