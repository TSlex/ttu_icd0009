using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF;
using DAL.App.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;

namespace DAL.App.Repositories
{
    public class StudentSubjectRepo : BaseRepo<Domain.StudentSubject, StudentSubject, ApplicationDbContext>,
        IStudentSubjectRepo
    {
        public StudentSubjectRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.StudentSubject, StudentSubject>())
        {
        }
    }
}