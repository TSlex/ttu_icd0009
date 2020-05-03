using System;
using BLL.App.DTO;
using BLL.App.Services;
using BLL.Base;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Http;

namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppBLL(IAppUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IBlockedProfileService BlockedProfiles => GetService<IBlockedProfileService>(() => new BlockedProfileService(UnitOfWork));

        public IChatMemberService ChatMembers => GetService<IChatMemberService>(() => new ChatMemberService(UnitOfWork));

        public IChatRoleService ChatRoles => GetService<IChatRoleService>(() => new ChatRoleService(UnitOfWork));

        public IChatRoomService ChatRooms => GetService<IChatRoomService>(() => new ChatRoomService(UnitOfWork, _httpContextAccessor));

        public ICommentService Comments => GetService<ICommentService>(() => new CommentService(UnitOfWork));

        public IFavoriteService Favorites => GetService<IFavoriteService>(() => new FavoriteService(UnitOfWork));
        
        public IFollowerService Followers => GetService<IFollowerService>(() => new FollowerService(UnitOfWork));

        public IGiftService Gifts => GetService<IGiftService>(() => new GiftService(UnitOfWork));

        public IMessageService Messages => GetService<IMessageService>(() => new MessageService(UnitOfWork));

        public IPostService Posts => GetService<IPostService>(() => new PostService(UnitOfWork));

        public IProfileGiftService ProfileGifts => GetService<IProfileGiftService>(() => new ProfileGiftService(UnitOfWork));
        
        public IProfileRankService ProfileRanks => GetService<IProfileRankService>(() => new ProfileRankService(UnitOfWork));
        
        public IProfileService Profiles => GetService<IProfileService>(() => new ProfileService(UnitOfWork));
        
        public IRankService Ranks => GetService<IRankService>(() => new RankService(UnitOfWork));

        public IImageService Images => GetService<IImageService>(() => new ImageService(UnitOfWork));
        
        public IFeedService FeedService => GetService<IFeedService>(() => new FeedService(UnitOfWork));
    }
}