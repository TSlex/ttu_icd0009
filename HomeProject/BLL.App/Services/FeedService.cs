using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class FeedService: BaseService, IFeedService
    {
        private readonly IAppUnitOfWork _uow;
        private readonly IBaseBLLMapper<DAL.App.DTO.Post, Post> _mapper;
        
        public FeedService(IAppUnitOfWork uow)
        {
            _uow = uow;
            _mapper = new PostMapper();
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
    }
}