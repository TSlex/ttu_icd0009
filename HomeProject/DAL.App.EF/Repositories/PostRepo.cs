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

        public async Task<Post> FindAsync(Guid? id)
        {
            return Mapper.Map(await RepoDbContext.Posts
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id));
        }
    }
}