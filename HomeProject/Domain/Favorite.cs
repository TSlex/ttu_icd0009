﻿using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace Domain
{
    public class Favorite : DomainEntityBaseMetadata
    {
        public Guid ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }

        public Guid PostId { get; set; } = default!;
        public Post? Post { get; set; }

        //what content user actually likes
        public string? PostTitle { get; set; }
        public Guid? PostImageId { get; set; }
        public string? PostDescription { get; set; }
    }
}