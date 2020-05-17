using System;
using System.Collections.Generic;
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
    public class ChatMemberRepo : BaseRepo<Domain.ChatMember, ChatMember, ApplicationDbContext>, IChatMemberRepo
    {
        public ChatMemberRepo(ApplicationDbContext dbContext) :
            base(dbContext, new ChatMemberMapper())
        {
        }

        public override async Task<ChatMember> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet
                .Where(member => member.Id == id)
                .Include(member => member.ChatRole)
                .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<ChatMember>> RoomAllAsync(Guid chatRoomId)
        {
            return (await RepoDbContext.ChatMembers
                    .Where(member => member.ChatRoomId == chatRoomId)
                    .Select(member => new Domain.ChatMember()
                    {
                        Id = member.Id,
                        Profile = new Domain.Profile()
                        {
                            UserName = member.Profile!.UserName,
//                            ProfileAvatarUrl = member.Profile!.ProfileAvatarUrl
                        },
                        ChatRole = new Domain.ChatRole()
                        {
                            RoleTitle = member.ChatRole!.RoleTitle
                        }
                    })
                    .ToListAsync())
                .Select(member => Mapper.Map(member));
        }
    }
}