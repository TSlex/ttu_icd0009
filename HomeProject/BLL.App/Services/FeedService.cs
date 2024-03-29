﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using ee.itcollege.aleksi.BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using ee.itcollege.aleksi.Contracts.BLL.Base.Mappers;

namespace BLL.App.Services
{
    public class FeedService: BaseService, IFeedService
    {
        private readonly IAppUnitOfWork _uow;
        private readonly IBaseBLLMapper<DAL.App.DTO.Post, Post> _mapper;
        
        public FeedService(IAppUnitOfWork uow)
        {
            _uow = uow;
            _mapper = new UniversalBLLMapper<DAL.App.DTO.Post, Post>();
        }

        public async Task<Feed> GetUserFeedAsync(Guid userId)
        {
            return new Feed()
            {
                Posts = (await _uow.Posts.GetUserFollowsPostsAsync(userId)).Select(post => _mapper.Map(post)).ToList()
            };
        }
        
        public async Task<Feed> GetCommonFeedAsync()
        {
            return new Feed()
            {
                Posts = (await _uow.Posts.GetCommonFeedAsync()).Select(post => _mapper.Map(post)).ToList()
            };
        }

        public async Task<int> GetUserCount(Guid userId)
        {
            return await _uow.Posts.GetUserFollowsPostsCount(userId);
        }

        public async Task<int> GetCount()
        {
            return await _uow.Posts.GetCommonPostsCount();
        }

        public async Task<Feed> GetUser10ByPage(Guid userId, int pageNumber)
        {
            return new Feed()
            {
                Posts = (await _uow.Posts.GetUserFollowsPostsByPage(userId, pageNumber, 10)).Select(post => _mapper.Map(post)).ToList()
            };
        }

        public async Task<Feed> Get10ByPage(int pageNumber)
        {
            return new Feed()
            {
                Posts = (await _uow.Posts.GetCommonFeedByPage(pageNumber, 10)).Select(post => _mapper.Map(post)).ToList()
            };
        }
    }
}