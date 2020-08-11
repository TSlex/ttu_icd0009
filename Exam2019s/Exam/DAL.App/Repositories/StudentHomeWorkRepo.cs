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
    public class StudentHomeWorkRepo : BaseRepo<Domain.StudentHomeWork, StudentHomeWork, ApplicationDbContext>,
        IStudentHomeWorkRepo
    {
        public StudentHomeWorkRepo(ApplicationDbContext dbContext) : base(
            dbContext, new UniversalDALMapper<Domain.StudentHomeWork, StudentHomeWork>())
        {
        }
        
        public override async Task<IEnumerable<StudentHomeWork>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(entity => entity.MasterId == id || entity.Id == id).ToListAsync())
                .Select(Mapper.Map);
        }
    }
}