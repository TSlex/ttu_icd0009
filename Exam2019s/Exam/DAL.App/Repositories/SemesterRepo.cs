using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF;
using DAL.App.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class SemesterRepo : BaseRepo<Domain.Semester, Semester, ApplicationDbContext>,
        ISemesterRepo
    {
        public SemesterRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.Semester, Semester>())
        {
        }
        
        public override async Task<IEnumerable<Semester>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(entity => entity.MasterId == id || entity.Id == id).ToListAsync())
                .Select(Mapper.Map);
        }
    }
}