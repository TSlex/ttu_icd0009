﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface ICommentService: IBaseEntityService<global::DAL.App.DTO.Comment, Comment>
    {
        Task<IEnumerable<Comment>> AllByIdPageAsync(Guid postId, int pageNumber, int count);
    }
}