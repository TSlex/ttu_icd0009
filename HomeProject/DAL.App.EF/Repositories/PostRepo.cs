using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Helpers;
using DAL.Mappers;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Post = DAL.App.DTO.Post;

namespace DAL.Repositories
{
    public class PostRepo : BaseRepo<Domain.Post, Post, ApplicationDbContext>, IPostRepo
    {
        private readonly BaseDALMapper<Image, DAL.App.DTO.Image> _imageMapper;
        
        public PostRepo(ApplicationDbContext dbContext) :
            base(dbContext, new PostMapper())
        {
            _imageMapper = new BaseDALMapper<Image, DAL.App.DTO.Image>();
        }

        public override async Task<IEnumerable<Post>> AllAsync()
        {
            return (await RepoDbContext.Posts
                .Include(post => post.Profile)
                .Include(p => p.PostImage)
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

        public async Task<Post> GetNoIncludes(Guid id, Guid? requesterId)
        {
            return (await RepoDbContext.Posts
                .Where(post => post.Id == id).Select(post => new Post()
                {
                    Id = post.Id,
                    ProfileId = post.ProfileId,
                    PostTitle = post.PostTitle,
                    PostDescription = post.PostDescription,
                    PostImageId = post.PostImageId,
                    PostImage = _imageMapper.Map(post.PostImage),
                    PostPublicationDateTime = post.PostPublicationDateTime,
//                    PostImageUrl = post.PostImageUrl,
                    PostCommentsCount = post.Comments!.Count,
                    PostFavoritesCount = post.Favorites!.Count,
                    IsUserFavorite = requesterId != null && post.Favorites.Select(favorite => favorite.ProfileId)
                                         .Contains((Guid) requesterId),
                    Profile = new DAL.App.DTO.Profile()
                    {
                        UserName = post.Profile!.UserName
                    }
                }).FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<Post>> GetUserFollowsPostsAsync(Guid userId)
        {
            return (await RepoDbContext.Posts
                .Where(post => post.Profile!.Followers
                                   .Select(follower => follower.FollowerProfileId)
                                   .Contains(userId) || post.ProfileId == userId)
                .Select(post => new Post()
                {
                    Id = post.Id,
                    ProfileId = post.ProfileId,
                    PostTitle = post.PostTitle,
                    PostDescription = post.PostDescription,
                    PostImageId = post.PostImageId,
                    PostPublicationDateTime = post.PostPublicationDateTime,
//                    PostImageUrl = post.PostImageUrl,
                    PostCommentsCount = post.Comments!.Count,
                    PostFavoritesCount = post.Favorites!.Count,
                    IsUserFavorite = post.Favorites.Select(favorite => favorite.ProfileId).Contains(userId),
                    Profile = new DAL.App.DTO.Profile()
                    {
                        UserName = post.Profile!.UserName
                    }
                })
                .ToListAsync());
        }

        public async Task<IEnumerable<Post>> GetCommonFeedAsync()
        {
            return (await RepoDbContext.Posts
                    .Select(post => new Domain.Post()
                    {
                        Id = post.Id,
                        ProfileId = post.ProfileId,
                        PostTitle = post.PostTitle,
                        PostDescription = post.PostDescription,
                        PostImageId = post.PostImageId,
                        PostPublicationDateTime = post.PostPublicationDateTime,
//                        PostImageUrl = post.PostImageUrl,
                        PostCommentsCount = post.Comments!.Count,
                        PostFavoritesCount = post.Favorites!.Count,
                        Profile = new Profile()
                        {
                            UserName = post.Profile!.UserName
                        }
                    })
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        public async Task<int> GetByUserCount(Guid userId)
        {
            return await RepoDbContext.Posts.CountAsync(post => post.ProfileId == userId);
        }

        public async Task<IEnumerable<Post>> GetUserByPage(Guid userId, int pageNumber, int onPageCount,
            Guid? requesterId)
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
                .Select(post => new Post()
                {
                    Id = post.Id,
                    ProfileId = post.ProfileId,
                    PostTitle = post.PostTitle,
                    PostDescription = post.PostDescription,
                    PostImageId = post.PostImageId,
                    PostPublicationDateTime = post.PostPublicationDateTime,
//                    PostImageUrl = post.PostImageUrl,
                    PostCommentsCount = post.Comments!.Count,
                    PostFavoritesCount = post.Favorites!.Count,
                    IsUserFavorite = requesterId != null && post.Favorites.Select(favorite => favorite.ProfileId)
                                         .Contains((Guid) requesterId),
                    Profile = new DAL.App.DTO.Profile()
                    {
                        UserName = post.Profile!.UserName
                    }
                })
                .Skip(startIndex)
                .Take(onPageCount)
                .ToListAsync());
        }

        public async Task<int> GetCommentsCount(Guid id)
        {
            return (await GetNoIncludes(id, null)).PostCommentsCount;
        }


        public async Task<int> GetFavoritesCount(Guid id)
        {
            return (await GetNoIncludes(id, null)).PostFavoritesCount;
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
                .Select(post => new Post()
                {
                    Id = post.Id,
                    ProfileId = post.ProfileId,
                    PostTitle = post.PostTitle,
                    PostDescription = post.PostDescription,
                    PostImageId = post.PostImageId,
                    PostPublicationDateTime = post.PostPublicationDateTime,
//                    PostImageUrl = post.PostImageUrl,
                    PostCommentsCount = post.Comments!.Count,
                    PostFavoritesCount = post.Favorites!.Count,
                    IsUserFavorite = post.Favorites.Select(favorite => favorite.ProfileId).Contains(userId),
                    Profile = new DAL.App.DTO.Profile()
                    {
                        UserName = post.Profile!.UserName
                    }
                })
                .Skip(startIndex)
                .Take(onPageCount)
                .ToListAsync());
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
                    ProfileId = post.ProfileId,
                    PostTitle = post.PostTitle,
                    PostDescription = post.PostDescription,
                    PostImageId = post.PostImageId,
                    PostPublicationDateTime = post.PostPublicationDateTime,
//                    PostImageUrl = post.PostImageUrl,
                    PostCommentsCount = post.Comments!.Count,
                    PostFavoritesCount = post.Favorites!.Count,
                    Profile = new Profile()
                    {
                        UserName = post.Profile!.UserName
                    }
                })
                .Skip(startIndex)
                .Take(onPageCount)
                .ToListAsync()).Select(post => Mapper.Map(post));
        }

        public async Task<bool> IsUserFavorite(Guid postId, Guid userId)
        {
            return (await RepoDbSet.FirstOrDefaultAsync(post => post.Id == postId &&
                                                                post.Favorites.Select(favorite => favorite.ProfileId)
                                                                    .Contains(userId))) != null;
        }

        public override Post Remove(Post entity)
        {
            var comments = RepoDbContext.Comments.Where(comment => comment.PostId == entity.Id).ToList();

            foreach (var comment in comments)
            {
                RepoDbContext.Comments.Remove(comment);
            }

            var favorites = RepoDbContext.Favorites.Where(favorite => favorite.PostId == entity.Id).ToList();

            foreach (var favorite in favorites)
            {
                RepoDbContext.Favorites.Remove(favorite);
            }
            
            var imageRecord = RepoDbContext.Images.FirstOrDefault(image => image.Id == entity.PostImageId);

            if (imageRecord != null)
            {
                RepoDbContext.Images.Remove(imageRecord);
            }

            return base.Remove(entity);
        }
    }
}