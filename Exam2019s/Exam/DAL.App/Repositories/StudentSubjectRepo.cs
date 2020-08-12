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
    public class StudentSubjectRepo : BaseRepo<Domain.StudentSubject, StudentSubject, ApplicationDbContext>,
        IStudentSubjectRepo
    {
        public StudentSubjectRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.StudentSubject, StudentSubject>())
        {
        }
        
        public override async Task<IEnumerable<StudentSubject>> AllAsync()
        {
            return (await RepoDbSet.IgnoreQueryFilters().Where(entity => entity.DeletedAt == null).ToListAsync())
                .Select(Mapper.Map);
        }

        public override async Task<StudentSubject> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.IgnoreQueryFilters().FirstOrDefaultAsync(entity => entity.Id == id && entity.DeletedAt == null));
        }
        
        public override async Task<IEnumerable<StudentSubject>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.IgnoreQueryFilters().Where(entity => entity.MasterId == id || entity.Id == id).ToListAsync())
                .Select(Mapper.Map);
        }
        
        public override async Task<IEnumerable<StudentSubject>> AllAdminAsync()
        {
            return (await RepoDbSet.IgnoreQueryFilters()
                    .Include(entity => entity.Student)
                    .Include(entity => entity.Subject)
                    .Where(entity => entity.MasterId == null).ToListAsync())
                .Select(Mapper.Map);
        }

        public override async Task<StudentSubject> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.IgnoreQueryFilters()
                .Include(entity => entity.Student)
                .Include(entity => entity.Subject)
                .FirstOrDefaultAsync(entity => entity.Id == id));
        }
    }
}