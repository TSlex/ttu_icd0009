using System;
using Contracts.DAL.App.Repositories;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork
    {
        IBlockedProfileRepo BlockedProfile { get; }
        IChatMemberRepo ChatMemberRepo { get; }
        IChatRoleRepo ChatRoleRepo { get; }
        IChatRoomRepo ChatRoomRepo { get; }
        ICommentRepo CommentRepo { get; }
        IFavoriteRepo FavoriteRepo { get; }
        IFollowerRepo FollowerRepo { get; }
        IGiftRepo GiftRepo { get; }
        IMessageRepo MessageRepo { get; }
        IPostRepo PostRepo { get; }
        IProfileGiftRepo ProfileGiftRepo { get; }
        IProfileRankRepo ProfileRankRepo { get; }
        IProfileRepo ProfileRepo { get; }
        IRankRepo RankRepo { get; }
    }
}