using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommentRepo : BaseRepo<Domain.Comment, Comment, ApplicationDbContext>, ICommentRepo
    {
        public CommentRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new CommentMapper())
        {
        }

        public override async Task<IEnumerable<Comment>> AllAsync()
        {
            return (await RepoDbContext.Comments.Where(comment => comment.DeletedAt == null)
                .Include(post => post.Profile)
                .ToListAsync()).Select(comment => Mapper.Map(comment));
        }

        public async Task<Comment> FindAsync(Guid? id)
        {
            return Mapper.Map(await RepoDbContext.Comments
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id));
        }

        public async Task<IEnumerable<Comment>> AllByIdPageAsync(Guid postId, int pageNumber, int count)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * count;

            if (pageIndex < 0)
            {
                return new Comment[] { };
            }

            return (await RepoDbContext.Comments
                    .Where(comment => comment.PostId == postId 
                                      && comment.DeletedAt == null)
                    .Include(comment => comment.Profile)
                    .OrderByDescending(comment => comment.CommentDateTime)
                    .Skip(startIndex)
                    .Take(count)
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }
        
        public override async Task<IEnumerable<Comment>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id).ToListAsync()).Select(record => Mapper.Map(record));
        }
    }
}