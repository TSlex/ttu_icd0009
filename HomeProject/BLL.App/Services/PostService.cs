using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.Base.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class PostService : BaseEntityService<IPostRepo, DAL.App.DTO.Post, Post>, IPostService
    {
        public PostService(IAppUnitOfWork uow) :
            base(uow.Posts, new BaseBLLMapper<DAL.App.DTO.Post, Post>())
        {
        }
    }
}