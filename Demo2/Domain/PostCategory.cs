﻿using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class PostCategory : DomainEntity
    {
        [MaxLength(36)] public string PostId { get; set; } = default!;
        public Post? Post { get; set; }

        [MaxLength(36)] public string CategoryId { get; set; } = default!;
        public Category? Category { get; set; }
    }
}