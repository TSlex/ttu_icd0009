using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ICommentRepo : IBaseRepo<Comment>
    {
        Task<IEnumerable<Comment>> AllByIdPageAsync(Guid postId, int pageNumber, int count);
    }
}