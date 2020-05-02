using System;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        IBlockedProfileRepo BlockedProfiles { get; }
        IChatMemberRepo ChatMembers { get; }
        IChatRoleRepo ChatRoles { get; }
        IChatRoomRepo ChatRooms { get; }
        ICommentRepo Comments { get; }
        IFavoriteRepo Favorites { get; }
        IFollowerRepo Followers { get; }
        IGiftRepo Gifts { get; }
        IMessageRepo Messages { get; }
        IPostRepo Posts { get; }
        IProfileGiftRepo ProfileGifts { get; }
        IProfileRankRepo ProfileRanks { get; }
        IProfileRepo Profiles { get; }
        IRankRepo Ranks { get; }
        IImageRepo Images { get; }
    }
}