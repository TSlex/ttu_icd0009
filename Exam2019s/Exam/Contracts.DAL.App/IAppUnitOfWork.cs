using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        ITemplateRepo Templates{ get; }
    }
}