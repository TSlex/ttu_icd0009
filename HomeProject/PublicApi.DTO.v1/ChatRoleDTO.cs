﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class ChatRoleDTO
    {
        public string RoleTitle { get; set; } = default!;
    }
}