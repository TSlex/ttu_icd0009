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

        public IBlockedProfileRepo BlockedProfile =>
            GetRepository<IBlockedProfileRepo>(() => new BlockedProfileRepo(UOWDbContext));

        public IChatMemberRepo ChatMemberRepo =>
            GetRepository<IChatMemberRepo>(() => new ChatMemberRepo(UOWDbContext));

        public IChatRoleRepo ChatRoleRepo =>
            GetRepository<IChatRoleRepo>(() => new ChatRoleRepo(UOWDbContext));

        public IChatRoomRepo ChatRoomRepo =>
            GetRepository<IChatRoomRepo>(() => new ChatRoomRepo(UOWDbContext));

        public ICommentRepo CommentRepo =>
            GetRepository<ICommentRepo>(() => new CommentRepo(UOWDbContext));

        public IFavoriteRepo FavoriteRepo =>
            GetRepository<IFavoriteRepo>(() => new FavoriteRepo(UOWDbContext));

        public IFollowerRepo FollowerRepo =>
            GetRepository<IFollowerRepo>(() => new FollowerRepo(UOWDbContext));

        public IGiftRepo GiftRepo => GetRepository<IGiftRepo>(() => new GiftRepo(UOWDbContext));

        public IMessageRepo MessageRepo =>
            GetRepository<IMessageRepo>(() => new MessageRepo(UOWDbContext));

        public IPostRepo PostRepo => GetRepository<IPostRepo>(() => new PostRepo(UOWDbContext));

        public IProfileGiftRepo ProfileGiftRepo =>
            GetRepository<IProfileGiftRepo>(() => new ProfileGiftRepo(UOWDbContext));

        public IProfileRankRepo ProfileRankRepo =>
            GetRepository<IProfileRankRepo>(() => new ProfileRankRepo(UOWDbContext));

        public IProfileRepo ProfileRepo =>
            GetRepository<IProfileRepo>(() => new ProfileRepo());

        public IRankRepo RankRepo => GetRepository<IRankRepo>(() => new RankRepo(UOWDbContext));
    }
}