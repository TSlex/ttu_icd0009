using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF;
using DAL.App.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;

namespace DAL.App.Repositories
{
    public class SubjectRepo : BaseRepo<Domain.Subject, Subject, ApplicationDbContext>,
        ISubjectRepo
    {
        public SubjectRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.Subject, Subject>())
        {
        }
    }
}