using DAL.App.DTO;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using DAL.Mappers;

namespace DAL.Repositories
{
    public class ProfileRepo : BaseRepo<Domain.Profile, Profile, ApplicationDbContext>, IProfileRepo
    {
        public ProfileRepo(ApplicationDbContext dbContext) 
            : base(dbContext, new ProfileMapper())
        {
        }
    }
}