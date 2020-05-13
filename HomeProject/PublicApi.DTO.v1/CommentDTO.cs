﻿using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace PublicApi.DTO.v1
{
    public class CommentGetDTO
    {
        public Guid Id { get; set; } = default!;
        
        public string UserName { get; set; } = default!;
        public string? ProfileAvatarUrl { get; set; }

        public string CommentValue { get; set; } = default!;
        public DateTime CommentDateTime { get; set; }
    }

    public class CommentCreateDTO
    {
        public Guid PostId { get; set; } = default!;

        [MaxLength(300)] [MinLength(1)] public string CommentValue { get; set; } = default!;
    }

    public class CommentEditDTO
    {
        public Guid Id { get; set; } = default!;

        [MaxLength(300)] [MinLength(1)] public string CommentValue { get; set; } = default!;
    }
}