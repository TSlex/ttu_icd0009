using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.aleksi.DAL.Base.EF.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommentRepo : BaseRepo<Domain.Comment, Comment, ApplicationDbContext>, ICommentRepo
    {
        public CommentRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new UniversalDALMapper<Domain.Comment, Comment>())
        {
        }

        public override async Task<Comment> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id));
        }

        public override async Task<IEnumerable<Comment>> AllAsync()
        {
            return (await GetQuery()
                .Where(comment => comment.DeletedAt == null)
                .ToListAsync()).Select(comment => Mapper.Map(comment));
        }

        public override async Task<Comment> FindAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .Include(p => p.Post)
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

            return (await GetQuery()
                    .Where(comment => comment.PostId == postId 
                                      && comment.DeletedAt == null)
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
        
        private IQueryable<Domain.Comment> GetQuery()
        {
            return RepoDbSet
                .Include(comment => comment.Profile)
                .AsQueryable();
        }
    }
}