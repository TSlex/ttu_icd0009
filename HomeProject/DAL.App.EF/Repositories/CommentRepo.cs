using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommentRepo : BaseRepo<Domain.Comment, Comment, ApplicationDbContext>, ICommentRepo
    {
        public CommentRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new BaseDALMapper<Domain.Comment, Comment>())
        {
        }

        public override async Task<IEnumerable<Comment>> AllAsync()
        {
            return (await RepoDbContext.Comments
                .Include(post => post.Profile)
                .ToListAsync()).Select(comment => Mapper.Map(comment));
        }

        public async Task<Comment> FindAsync(Guid? id)
        {
            return Mapper.Map(await RepoDbContext.Comments
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id));
        }
    }
}