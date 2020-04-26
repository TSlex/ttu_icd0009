﻿using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IPostService: IBaseEntityService<global::DAL.App.DTO.Post, Post>
    {
        Task<Post> GetPostFull(Guid id);
    }
}