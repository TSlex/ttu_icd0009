using System.Threading.Tasks;

namespace ee.itcollege.aleksi.Contracts.DAL.Base
{
    public interface IBaseUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}