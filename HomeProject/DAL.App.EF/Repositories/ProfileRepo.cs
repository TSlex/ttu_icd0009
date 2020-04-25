using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.DTO;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProfileRepo : BaseRepo<Domain.Profile, Profile, ApplicationDbContext>, IProfileRepo
    {
        public ProfileRepo(ApplicationDbContext dbContext) 
            : base(dbContext, new ProfileMapper())
        {
        }

        public override async Task<Profile> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.Include(profile => profile.Posts)
                .FirstOrDefaultAsync(profile => profile.Id == id));
        }
    }
}