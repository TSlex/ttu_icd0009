using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class RankRepo : BaseRepo<Domain.Rank, Rank, ApplicationDbContext>, IRankRepo
    {
        public RankRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new RankMapper())
        {
        }

        public async Task<Rank> FindByCodeAsync(string code)
        {
            return Mapper.Map(await RepoDbContext.Ranks.FirstOrDefaultAsync(rank => rank.RankCode == code));
        }
    }
}