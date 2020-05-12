﻿using System;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace BLL.App.DTO
{
    public class Favorite : DomainEntityBaseMetadata
    {
        [Display(Name = nameof(ProfileId), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public Guid ProfileId { get; set; } = default!;

        [Display(Name = nameof(Profile), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public ProfileFull? Profile { get; set; }

        [Display(Name = nameof(PostId), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public Guid PostId { get; set; } = default!;

        [Display(Name = nameof(Post), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public Post? Post { get; set; }

        //what content user actually likes
        [Display(Name = nameof(PostTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public string PostTitle { get; set; } = default!;

        [Display(Name = nameof(PostImageUrl), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public string? PostImageUrl { get; set; }

        [Display(Name = nameof(PostDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Favorites.Favorites))]
        public string? PostDescription { get; set; }
    }
}