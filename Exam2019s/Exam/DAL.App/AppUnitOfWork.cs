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

        public ITemplateRepo Templates => GetRepository<ITemplateRepo>(() => new TemplateRepo(UOWDbContext));
    }
}