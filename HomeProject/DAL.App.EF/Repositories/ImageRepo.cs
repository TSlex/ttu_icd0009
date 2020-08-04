using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.DTO;
using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ImageRepo : BaseRepo<Domain.Image, Image, ApplicationDbContext>, IImageRepo
    {
        public ImageRepo(ApplicationDbContext dbContext) :
            base(dbContext, new UniversalDALMapper<Domain.Image, Image>())
        {
        }

        public async Task<Image> FindProfileAsync(Guid userId)
        {
            return Mapper.Map(await RepoDbSet
                .FirstOrDefaultAsync(image => image.Profiles
                    .Select(profile => profile.Id)
                    .Contains(userId)));
        }

        public async Task<Image> FindPostAsync(Guid postId)
        {
            return Mapper.Map(await RepoDbSet
                .FirstOrDefaultAsync(image => image.Posts
                    .Select(post => post.Id)
                    .Contains(postId)));
        }

        public async Task<Image> FindGiftAsync(Guid giftId)
        {
            return Mapper.Map(await RepoDbSet
                .FirstOrDefaultAsync(image => image.Gifts
                    .Select(gift => gift.Id)
                    .Contains(giftId)));
        }

        public override async Task<IEnumerable<Image>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet
                    .Where(record => record.Id == id || record.MasterId == id)
                    .ToListAsync())
                .Select(record => Mapper.Map(record));
        }
    }
}