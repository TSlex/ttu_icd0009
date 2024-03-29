﻿using System.Threading.Tasks;
using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IChatRoleService: IBaseEntityService<global::DAL.App.DTO.ChatRole, ChatRole>
    {
        Task<ChatRole> FindAsync(string chatRoleTitle);
    }
}