using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Domain;
using Microsoft.EntityFrameworkCore;
using Post = DAL.App.DTO.Post;

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

        public async Task<Post> GetNoIncludes(Guid id)
        {
            var query = RepoDbContext.Posts.Where(post => post.Id == id).AsQueryable();

            var favoritesCount = await query.SelectMany(post => post.Favorites).CountAsync();
            var commentsCount = await query.SelectMany(post => post.Comments).CountAsync();

            var record = Mapper.Map(query.FirstOrDefault());

            record.PostFavoritesCount = favoritesCount;
            record.PostCommentsCount = commentsCount;

            return record;
        }

        public async Task<IEnumerable<Post>> GetUserFollowsPostsAsync(Guid userId)
        {
            return (await RepoDbContext.Posts
                    .Where(post => post.Profile!.Followers
                                       .Select(follower => follower.FollowerProfileId)
                                       .Contains(userId) || post.ProfileId == userId)
                    .Select(post => new Domain.Post()
                    {
                        Id = post.Id,
//                        Profile = post.Profile,
                        PostTitle = post.PostTitle,
                        PostDescription = post.PostDescription,
                        PostImageId = post.PostImageId,
                        PostPublicationDateTime = post.PostPublicationDateTime,
                        PostImageUrl = post.PostImageUrl,
                        PostCommentsCount = post.Comments!.Count,
                        PostFavoritesCount = post.Favorites!.Count,
                        Profile = new Profile()
                        {
                            UserName = post.Profile.UserName
                        }
                    })
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        public async Task<IEnumerable<Post>> GetCommonFeedAsync()
        {
            return (await RepoDbContext.Posts
                    .Select(post => new Domain.Post()
                    {
                        Id = post.Id,
                        PostTitle = post.PostTitle,
                        PostDescription = post.PostDescription,
                        PostImageId = post.PostImageId,
                        PostPublicationDateTime = post.PostPublicationDateTime,
                        PostImageUrl = post.PostImageUrl,
                        PostCommentsCount = post.Comments!.Count,
                        PostFavoritesCount = post.Favorites!.Count,
                        Profile = new Profile()
                        {
                            UserName = post.Profile.UserName
                        }
                    })
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        public async Task<int> GetByUserCount(Guid userId)
        {
            return await RepoDbContext.Posts.CountAsync(post => post.ProfileId == userId);
        }

        public async Task<IEnumerable<Post>> GetUserByPage(Guid userId, int pageNumber, int onPageCount)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * onPageCount;

            if (pageIndex < 0)
            {
                return new Post[] { };
            }

            return (await RepoDbContext.Posts
                    .Where(post => post.ProfileId == userId)
                    .OrderByDescending(post => post.PostPublicationDateTime)
//                    .Select(post => LimitPost(post))
                    .Select(post => new Domain.Post()
                    {
                        Id = post.Id,
                        PostTitle = post.PostTitle,
                        PostDescription = post.PostDescription,
                        PostImageId = post.PostImageId,
                        PostPublicationDateTime = post.PostPublicationDateTime,
                        PostImageUrl = post.PostImageUrl,
                        PostCommentsCount = post.Comments!.Count,
                        PostFavoritesCount = post.Favorites!.Count,
                        Profile = new Profile()
                        {
                            UserName = post.Profile.UserName
                        }
                    })
                    .Skip(startIndex)
                    .Take(onPageCount)
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        public async Task<int> GetCommentsCount(Guid id)
        {
            return (await GetNoIncludes(id)).PostCommentsCount;
        }


        public async Task<int> GetFavoritesCount(Guid id)
        {
            return (await GetNoIncludes(id)).PostFavoritesCount;
        }

        public async Task<int> GetUserFollowsPostsCount(Guid userId)
        {
            return await RepoDbContext.Posts
                .CountAsync(post => post.Favorites
                    .Select(favorite => favorite.ProfileId)
                    .Contains(userId));
        }

        public async Task<int> GetCommonPostsCount()
        {
            return await RepoDbContext.Posts.CountAsync();
        }

        public async Task<IEnumerable<Post>> GetUserFollowsPostsByPage(Guid userId, int pageNumber, int onPageCount)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * onPageCount;

            if (pageIndex < 0)
            {
                return new Post[] { };
            }

            return (await RepoDbContext.Posts
                    .Where(post => post.Profile!.Followers
                                       .Select(follower => follower.FollowerProfileId)
                                       .Contains(userId) || post.ProfileId == userId)
                    .OrderByDescending(post => post.PostPublicationDateTime)
                    .Select(post => new Domain.Post()
                    {
                        Id = post.Id,
                        PostTitle = post.PostTitle,
                        PostDescription = post.PostDescription,
                        PostImageId = post.PostImageId,
                        PostPublicationDateTime = post.PostPublicationDateTime,
                        PostImageUrl = post.PostImageUrl,
                        PostCommentsCount = post.Comments!.Count,
                        PostFavoritesCount = post.Favorites!.Count,
                        Profile = new Profile()
                        {
                            UserName = post.Profile.UserName
                        }
                    })
                    .Skip(startIndex)
                    .Take(onPageCount)
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        public async Task<IEnumerable<Post>> GetCommonFeedByPage(int pageNumber, int onPageCount)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * onPageCount;

            if (pageIndex < 0)
            {
                return new Post[] { };
            }

            return (await RepoDbContext.Posts
                    .OrderByDescending(post => post.PostPublicationDateTime)
                    .Select(post => new Domain.Post()
                    {
                        Id = post.Id,
                        PostTitle = post.PostTitle,
                        PostDescription = post.PostDescription,
                        PostImageId = post.PostImageId,
                        PostPublicationDateTime = post.PostPublicationDateTime,
                        PostImageUrl = post.PostImageUrl,
                        PostCommentsCount = post.Comments!.Count,
                        PostFavoritesCount = post.Favorites!.Count,
                        Profile = new Profile()
                        {
                            UserName = post.Profile.UserName
                        }
                    })
                    .Skip(startIndex)
                    .Take(onPageCount)
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        /*private static Domain.Post LimitPost(Domain.Post post)
        {
            var result = new Domain.Post
            {
                Id = post.Id,
                PostTitle = post.PostTitle,
                PostDescription = post.PostDescription,
                PostImageId = post.PostImageId,
                PostPublicationDateTime = post.PostPublicationDateTime,
                PostImageUrl = post.PostImageUrl,
                PostCommentsCount = post.Comments?.Count ?? 0,
                PostFavoritesCount = post.Favorites?.Count ?? 0
            };
            
            return result;
        }*/
    }
}