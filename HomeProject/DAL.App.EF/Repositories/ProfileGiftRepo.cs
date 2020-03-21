using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProfileGiftRepo : BaseRepo<ProfileGift>, IProfileGiftRepo
    {
        public ProfileGiftRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}