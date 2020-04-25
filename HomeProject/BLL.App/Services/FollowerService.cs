using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class FollowerService : BaseEntityService<IFollowerRepo, DAL.App.DTO.Follower, Follower>, IFollowerService
    {
        public FollowerService(IAppUnitOfWork uow) :
            base(uow.Followers, new FollowerMapper())
        {
        }
    }
}