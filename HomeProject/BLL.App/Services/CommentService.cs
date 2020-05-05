using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Comment>> AllByIdPageAsync(Guid postId, int pageNumber, int count)
        {
            return (await ServiceRepository.AllByIdPageAsync(postId, pageNumber, count)).Select(comment =>
                Mapper.Map(comment));
        }
    }
}