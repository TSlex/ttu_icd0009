﻿using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;

namespace DAL.Repositories
{
    public class ChatMemberRepo : BaseRepo<Domain.ChatMember, ChatMember, ApplicationDbContext>, IChatMemberRepo
    {
        public ChatMemberRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new ChatMemberMapper())
        {
        }
    }
}