using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PostRepo : BaseRepo<Domain.Post, Post, ApplicationDbContext>, IPostRepo
    {
        public PostRepo(ApplicationDbContext dbContext) :
            base(dbContext, new PostMapper())
        {
        }

        public override async Task<IEnumerable<Post>> AllAsync()
        {
            return (await RepoDbContext.Posts
                .Include(post => post.Profile)
                .ToListAsync()).Select(post => Mapper.Map(post));
        }

        public override async Task<Post> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbContext.Posts
                .Include(p => p.Profile)
                .Include(p => p.PostImage)
                .Include(p => p.Comments)
                .ThenInclude(comment => comment.Profile)
                .Include(p => p.Favorites)
                .FirstOrDefaultAsync(m => m.Id == id));
        }

        public async Task<IEnumerable<Post>> GetUserFollowsPostsAsync(Guid userId)
        {
            return (await RepoDbContext.Posts
                .Where(post => post.Profile!.Followers
                    .Select(follower => follower.FollowerProfileId)
                    .Contains(userId) || post.ProfileId == userId).Select(post => new Domain.Post()
                {
                    Id = post.Id,
//                    ProfileId = post.ProfileId,
                    PostTitle = post.PostTitle,
                    PostDescription = post.PostDescription,
                    PostImageUrl = post.PostImageUrl,
                    PostCommentsCount = post.Comments!.Count,
                    PostFavoritesCount = post.Favorites!.Count,
                    PostPublicationDateTime = post.PostPublicationDateTime,
                    Profile = post.Profile
                }).ToListAsync()).Select(post => Mapper.Map(post));
        }

        public async Task<IEnumerable<Post>> GetCommonFeedAsync()
        {
            return (await RepoDbContext.Posts.Select(post => new Domain.Post()
            {
                Id = post.Id,
//                ProfileId = post.ProfileId,
                PostTitle = post.PostTitle,
                PostDescription = post.PostDescription,
                PostImageUrl = post.PostImageUrl,
                PostCommentsCount = post.Comments!.Count,
                PostFavoritesCount = post.Favorites!.Count,
                PostPublicationDateTime = post.PostPublicationDateTime,
                Profile = post.Profile
            }).ToListAsync()).Select(post => Mapper.Map(post));
        }
    }
}