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
        
        public override async Task<IEnumerable<Semester>> AllAsync()
        {
            return (await RepoDbSet.IgnoreQueryFilters().Where(entity => entity.DeletedAt == null).ToListAsync())
                .Select(Mapper.Map);
        }

        public override async Task<Semester> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.IgnoreQueryFilters().FirstOrDefaultAsync(entity => entity.Id == id && entity.DeletedAt == null));
        }
        
        public override async Task<IEnumerable<Semester>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.IgnoreQueryFilters().Where(entity => entity.MasterId == id || entity.Id == id).ToListAsync())
                .Select(Mapper.Map);
        }
        
        public override async Task<IEnumerable<Semester>> AllAdminAsync()
        {
            return (await RepoDbSet.IgnoreQueryFilters().Where(entity => entity.MasterId == null).ToListAsync())
                .Select(Mapper.Map);
        }

        public override async Task<Semester> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.IgnoreQueryFilters().FirstOrDefaultAsync(entity => entity.Id == id));
        }
    }
}