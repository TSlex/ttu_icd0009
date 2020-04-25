using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class CommentService : BaseEntityService<ICommentRepo, DAL.App.DTO.Comment, Comment>, ICommentService
    {
        public CommentService(IAppUnitOfWork uow) :
            base(uow.Comments, new CommentMapper())
        {
        }
    }
}