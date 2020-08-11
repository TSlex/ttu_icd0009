using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF;
using DAL.App.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;

namespace DAL.App.Repositories
{
    public class StudentHomeWorkRepo : BaseRepo<Domain.StudentHomeWork, StudentHomeWork, ApplicationDbContext>,
        IStudentHomeWorkRepo
    {
        public StudentHomeWorkRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.StudentHomeWork, StudentHomeWork>())
        {
        }
    }
}