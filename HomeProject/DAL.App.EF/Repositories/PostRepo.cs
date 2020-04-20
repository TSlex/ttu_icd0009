using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PostRepo : BaseRepo<Post, ApplicationDbContext>, IPostRepo
    {
        public PostRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IEnumerable<Post>> AllAsync()
        {
            return await RepoDbContext.Posts
                .Include(post => post.Profile)
                .ToListAsync();
        }

        public async Task<Post> FindAsync(Guid? id)
        {
            return await RepoDbContext.Posts
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}