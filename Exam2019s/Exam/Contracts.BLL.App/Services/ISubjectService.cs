using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface ISubjectService : IBaseEntityService<DAL.App.DTO.Subject, Subject>
    {
        // custom service methods
    }
}