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
    public class SubjectRepo : BaseRepo<Domain.Subject, Subject, ApplicationDbContext>,
        ISubjectRepo
    {
        public SubjectRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.Subject, Subject>())
        {
        }
        
        public override async Task<IEnumerable<Subject>> AllAsync()
        {
            return (await RepoDbSet.IgnoreQueryFilters().Where(entity => entity.DeletedAt == null).ToListAsync())
                .Select(Mapper.Map);
        }

        public override async Task<Subject> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.IgnoreQueryFilters().FirstOrDefaultAsync(entity => entity.Id == id && entity.DeletedAt == null));
        }
        
        public override async Task<IEnumerable<Subject>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.IgnoreQueryFilters()
                    .Include(entity => entity.Teacher)
                    .Include(entity => entity.Semester)
                    .Where(entity => entity.MasterId == id || entity.Id == id).ToListAsync())
                .Select(Mapper.Map);
        }
        
        public override async Task<IEnumerable<Subject>> AllAdminAsync()
        {
            return (await RepoDbSet.IgnoreQueryFilters()
                    .Include(entity => entity.Teacher)
                    .Include(entity => entity.Semester)
                    .Where(entity => entity.MasterId == null)
                    .ToListAsync())
                .Select(Mapper.Map);
        }

        public override async Task<Subject> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.IgnoreQueryFilters()
                .Include(entity => entity.Teacher)
                .Include(entity => entity.Semester)
                .FirstOrDefaultAsync(entity => entity.Id == id));
        }
    }
}