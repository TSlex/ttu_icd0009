﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Base;

namespace Domain
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Post : DomainEntityMetadata
    {
        [ForeignKey(nameof(Author))]
        [MaxLength(36)]
        public string AuthorId { get; set; } = default!;
//        [ForeignKey(nameof(AuthorFk))
        public Author? Author { get; set; }
        
        [ForeignKey(nameof(CoAuthor))]
        [MaxLength(36)]
        public string CoAuthorId { get; set; } = default!;
        public Author? CoAuthor { get; set; }

        [MaxLength(4096)] 
        [MinLength(1)]
        public string Body { get; set; } = default!;


        public ICollection<PostCategory>? PostCategories { get; set; }
    }
}