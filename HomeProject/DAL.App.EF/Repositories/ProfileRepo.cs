using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.DTO;
using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using DAL.Helpers;
using DAL.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProfileRepo : BaseRepo<Domain.Profile, Profile, ApplicationDbContext>, IProfileRepo
    {
        private readonly UserManager<Domain.Profile> _userManager;

        public ProfileRepo(ApplicationDbContext dbContext, UserManager<Domain.Profile> userManager)
            : base(dbContext, new ProfileMapper())
        {
            _userManager = userManager;
        }

        public async Task<Profile> FindFullIncludeAsync(Guid id)
        {
            return Mapper.Map(await _userManager.Users
                .Include(profile => profile.Posts)
                .Include(profile => profile.Followed)
                .Include(profile => profile.Followers)
                .Include(profile => profile.ProfileGifts)
                .ThenInclude(gift => gift.Gift)
                .Include(profile => profile.ProfileRanks)
                .ThenInclude(rank => rank.Rank)
                .ThenInclude(rank => rank.RankTitle)
                .ThenInclude(title => title.Translations)
                .Include(profile => profile.ProfileRanks)
                .ThenInclude(rank => rank.Rank)
                .ThenInclude(rank => rank.RankDescription)
                .ThenInclude(desc => desc.Translations)
//                .Include(profile => profile.ProfileRanks)
//                .ThenInclude(rank => rank.Rank)
//                .ThenInclude(rank => rank!.NextRank)
                .FirstOrDefaultAsync(profile => profile.Id == id));
        }

        public async Task<Profile> FindNoIncludeAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet
                .Include(profile => profile.Posts!.Count)
                .Include(profile => profile.Followed!.Count)
                .Include(profile => profile.Followers!.Count)
                .FirstOrDefaultAsync(profile => profile.Id == id));
        }

        public async Task<Profile> FindByUsernameAsync(string username)
        {
            var result = await _userManager.Users
                .Where(profile => profile.UserName == username)
                .Select(profile => new
                {
                    value = profile,
                    postsCount = profile.Posts.Count,
                    followedCount = profile.Followed.Count,
                    followersCount = profile.Followers.Count
                }).FirstOrDefaultAsync();

            if (result?.value != null)
            {
                result.value.PostsCount = result.postsCount;
                result.value.FollowedCount = result.followedCount;
                result.value.FollowersCount = result.followersCount;
            }

            return Mapper.Map(result?.value);
        }

        public async Task IncreaseExperience(Guid userId, int amount)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return;
            }

            user.Experience += amount;

            await _userManager.UpdateAsync(user);
        }

        public override Task<Profile> UpdateAsync(Profile entity)
        {
            throw new NotImplementedException();

            /*var stamp = (await _userManager.FindByIdAsync(entity.Id.ToString())).SecurityStamp;
            var mappedEntity = Mapper.MapReverse(entity);
            
            mappedEntity.SecurityStamp = stamp;
            
            var result = await _userManager.UpdateAsync(mappedEntity);

            return result.Succeeded ? entity : null;*/
        }
    }
}