using System;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base;

namespace Contracts.BLL.App
{
    public interface IAppBLL : IBaseBLL
    {
        IBlockedProfileService BlockedProfiles { get; }
        IChatMemberService ChatMembers { get; }
        IChatRoleService ChatRoles { get; }
        IChatRoomService ChatRooms { get; }
        ICommentService Comments { get; }
        IFavoriteService Favorites { get; }
        IFollowerService Followers { get; }
        IGiftService Gifts { get; }
        IMessageService Messages { get; }
        IPostService Posts { get; }
        IProfileGiftService ProfileGifts { get; }
        IProfileRankService ProfileRanks { get; }
        IProfileService Profiles { get; }
        IRankService Ranks { get; }
        IFeedService FeedService { get; }
    }
}