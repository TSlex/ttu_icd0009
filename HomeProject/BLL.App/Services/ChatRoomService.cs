﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.App.Mappers;
using ee.itcollege.aleksi.BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using Microsoft.AspNetCore.Http;
using ChatRoom = BLL.App.DTO.ChatRoom;

namespace BLL.App.Services
{
    public class ChatRoomService : BaseEntityService<IChatRoomRepo, DAL.App.DTO.ChatRoom, ChatRoom>, IChatRoomService
    {
        private readonly IAppUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatRoomService(IAppUnitOfWork uow, IHttpContextAccessor httpContextAccessor) :
            base(uow.ChatRooms, new UniversalBLLMapper<DAL.App.DTO.ChatRoom, ChatRoom>())
        {
            _uow = uow;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid?> OpenOrCreateAsync(string username)
        {
            var otherProfile = await _uow.Profiles.FindByUsernameAsync(username);

            if (otherProfile == null)
            {
                return null;
            }

            var currentUser =
                await _uow.Profiles.FindByUsernameAsync(_httpContextAccessor.HttpContext.User!.Identity!.Name);

            var chatRoom = await _uow.ChatRooms.GetRoomWithUserAsync(otherProfile.Id, currentUser.Id);

            if (chatRoom == null)
            {
                chatRoom = _uow.ChatRooms.Add(new DAL.App.DTO.ChatRoom()
                {
                    /*ChatRoomTitle = "Chat of " + currentUser.UserName + " and " + otherProfile.UserName*/
                    ChatRoomTitle = string.Format(Resourses.BLL.App.DTO.ChatRooms.ChatRooms.DialogDefaultTitle,
                        currentUser.UserName, otherProfile.UserName)
                });

                var memberRole = await _uow.ChatRoles.FindAsync("Member");
                var creatorRole = await _uow.ChatRoles.FindAsync("Creator");

                _uow.ChatMembers.Add(new ChatMember()
                {
                    ProfileId = currentUser.Id,
                    ChatRoomId = chatRoom.Id,
                    ChatRoleId = creatorRole!.Id,
                    ChatRoomTitle = "Chat with " + otherProfile.UserName
                });

                _uow.ChatMembers.Add(new ChatMember()
                {
                    ProfileId = otherProfile.Id,
                    ChatRoomId = chatRoom.Id,
                    ChatRoleId = memberRole!.Id,
                    ChatRoomTitle = "Chat with " + currentUser.UserName
                });

                await _uow.SaveChangesAsync();
            }

            return chatRoom.Id;
        }

        public async Task<IEnumerable<ChatRoom>> AllAsync(Guid userId)
        {
            return (await ServiceRepository.AllAsync(userId))
                .Where(room => FilterByMemberDeleted(room, userId))
                .Select(room => Mapper.Map(room));
        }

        public override async Task<ChatRoom> FindAsync(Guid id)
        {
            var result = await ServiceRepository.FindAsync(id);

            result.Messages = result.Messages
                .Where(message =>
                    message.DeletedAt == null &&
                    message.MasterId == null)
                .ToList();

            return Mapper.Map(result);
        }

        private static bool FilterByMemberDeleted(DAL.App.DTO.ChatRoom chatRoom, Guid userId)
        {
            var member = chatRoom?.ChatMembers.FirstOrDefault(chatMember => chatMember.ProfileId == userId);

            return member?.DeletedAt == null;
        }

        public async Task<bool> IsRoomMemberAsync(Guid chatRoomId, Guid userId)
        {
            return await ServiceRepository.IsRoomMemberAsync(chatRoomId, userId);
        }

        public async Task<bool> IsRoomAdministratorAsync(Guid chatRoomId, Guid userId)
        {
            return await ServiceRepository.IsRoomAdministratorAsync(chatRoomId, userId);
        }
    }
}