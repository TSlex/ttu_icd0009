using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF;
using DAL.App.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;

namespace DAL.App.Repositories
{
    public class SemesterRepo : BaseRepo<Domain.Semester, Semester, ApplicationDbContext>,
        ISemesterRepo
    {
        public SemesterRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.Semester, Semester>())
        {
        }
    }
}