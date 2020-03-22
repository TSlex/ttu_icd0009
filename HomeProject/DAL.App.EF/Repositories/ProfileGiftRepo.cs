using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProfileGiftRepo : BaseRepo<ProfileGift, ApplicationDbContext>, IProfileGiftRepo
    {
        public ProfileGiftRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}