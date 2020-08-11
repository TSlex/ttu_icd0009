using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        IHomeWorkRepo HomeWorks{ get; }
        ISemesterRepo Semesters{ get; }
        ISubjectRepo Subjects{ get; }
        IStudentSubjectRepo StudentSubjects{ get; }
        IStudentHomeWorkRepo StudentHomeWorks{ get; }
    }
}