using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.Base;
using DAL.Base.EF;
using DAL.Repositories;

namespace DAL
{
    public class AppUnitOfWork : EFBaseUnitOfWork<ApplicationDbContext>, IAppUnitOfWork
    {
        public AppUnitOfWork(ApplicationDbContext uowDbContext) : base(uowDbContext)
        {
        }

        public IBlockedProfileRepo BlockedProfiles =>
            GetRepository<IBlockedProfileRepo>(() => new BlockedProfileRepo(UOWDbContext));

        public IChatMemberRepo ChatMembers =>
            GetRepository<IChatMemberRepo>(() => new ChatMemberRepo(UOWDbContext));

        public IChatRoleRepo ChatRoles =>
            GetRepository<IChatRoleRepo>(() => new ChatRoleRepo(UOWDbContext));

        public IChatRoomRepo ChatRooms =>
            GetRepository<IChatRoomRepo>(() => new ChatRoomRepo(UOWDbContext));

        public ICommentRepo Comments =>
            GetRepository<ICommentRepo>(() => new CommentRepo(UOWDbContext));

        public IFavoriteRepo Favorites =>
            GetRepository<IFavoriteRepo>(() => new FavoriteRepo(UOWDbContext));

        public IFollowerRepo Followers =>
            GetRepository<IFollowerRepo>(() => new FollowerRepo(UOWDbContext));

        public IGiftRepo Gifts => GetRepository<IGiftRepo>(() => new GiftRepo(UOWDbContext));

        public IMessageRepo Messages =>
            GetRepository<IMessageRepo>(() => new MessageRepo(UOWDbContext));

        public IPostRepo Posts => GetRepository<IPostRepo>(() => new PostRepo(UOWDbContext));

        public IProfileGiftRepo ProfileGifts =>
            GetRepository<IProfileGiftRepo>(() => new ProfileGiftRepo(UOWDbContext));

        public IProfileRankRepo ProfileRanks =>
            GetRepository<IProfileRankRepo>(() => new ProfileRankRepo(UOWDbContext));

        public IProfileRepo Profiles =>
            GetRepository<IProfileRepo>(() => new ProfileRepo(UOWDbContext));

        public IRankRepo Ranks => GetRepository<IRankRepo>(() => new RankRepo(UOWDbContext));
    }
}