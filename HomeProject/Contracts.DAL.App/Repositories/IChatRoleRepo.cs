using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IChatRoleRepo : IBaseRepo<ChatRole>
    {
        Task<ChatRole> FindAsync(string chatRoleTitle);
    }
}