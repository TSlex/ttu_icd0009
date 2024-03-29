﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using ee.itcollege.aleksi.BLL.Base.Services;
using Contracts.BLL.App.Services;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class ChatMemberService : BaseEntityService<IChatMemberRepo, DAL.App.DTO.ChatMember, ChatMember>,
        IChatMemberService
    {
        public ChatMemberService(IAppUnitOfWork uow) :
            base(uow.ChatMembers, new UniversalBLLMapper<DAL.App.DTO.ChatMember, ChatMember>())
        {
        }


        public async Task<ChatMember> FindByUserAndRoomAsync(Guid userId, Guid chatRoomId)
        {
            return Mapper.Map(await ServiceRepository.FindByUserAndRoomAsync(userId, chatRoomId));
        }

        public async Task<IEnumerable<ChatMember>> RoomAllAsync(Guid chatRoomId)
        {
            return (await ServiceRepository.RoomAllAsync(chatRoomId)).Select(member => Mapper.Map(member));
        }
    }
}