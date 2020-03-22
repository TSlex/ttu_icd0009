using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommentRepo : BaseRepo<Comment, ApplicationDbContext>, ICommentRepo
    {
        public CommentRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IEnumerable<Comment>> AllAsync()
        {
            return await RepoDbContext.Comments
                .Include(post => post.Profile)
                .ToListAsync();
        }

        public async Task<Comment> FindAsync(Guid? id)
        {
            return await RepoDbContext.Comments
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}