﻿using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PostRepo : BaseRepo<Post>, IPostRepo
    {
        public PostRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}