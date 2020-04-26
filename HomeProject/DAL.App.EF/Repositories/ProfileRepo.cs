using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.DTO;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProfileRepo : BaseRepo<Domain.Profile, Profile, ApplicationDbContext>, IProfileRepo
    {
        private readonly UserManager<Domain.Profile> _userManager;
        
        public ProfileRepo(ApplicationDbContext dbContext, UserManager<Domain.Profile> userManager) 
            : base(dbContext, new ProfileMapper())
        {
            _userManager = userManager;
        }

        public override async Task<Profile> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet
                .Include(profile => profile.Posts)
                .Include(profile => profile.Followed)
                .Include(profile => profile.Followers)
                .FirstOrDefaultAsync(profile => profile.Id == id));
        }

        public async Task<Profile> FindByUsernameAsync(string username)
        {
            return Mapper.Map(await _userManager.FindByNameAsync(username));
        }
    }
}