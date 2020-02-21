﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Base;
using Domain.Identity;

namespace Domain
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Author : DomainEntityMetadata
    {
        
        [MaxLength(36)]
        public string AppUserId { get; set; } = default!;
        public AppUser? AppUser { get; set; }
        
        [MaxLength(2000)] public string FirstName { get; set; } = "";
        [MaxLength(2000)] public string LastName { get; set; } = "";

        [MaxLength(36)] public string? AuthorPictureId { get; set; } = default!;
        public AuthorPicture? AuthorPicture { get; set; }

        [NotMapped] public string? FirstLastName { get; set; }

        [InverseProperty(nameof(Post.Author))] public ICollection<Post>? AuthoredPosts { get; set; }

        [InverseProperty(nameof(Post.CoAuthor))]
        public ICollection<Post>? CoAuthoredPosts { get; set; }
    }
}