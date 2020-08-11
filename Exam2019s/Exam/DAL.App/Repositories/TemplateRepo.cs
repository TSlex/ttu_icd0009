using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF;
using DAL.App.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;

namespace DAL.App.Repositories
{
    public class TemplateRepo : BaseRepo<Domain.Template, Template, ApplicationDbContext>,
        ITemplateRepo
    {
        public TemplateRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.Template, Template>())
        {
        }
    }
}