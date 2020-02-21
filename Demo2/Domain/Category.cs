using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Base;

namespace Domain
{
    public class Category : DomainEntityMetadata
    {
        [MaxLength(128)] public string Name { get; set; } = default!;

        [MaxLength(36)] public string? ParentCategoryId { get; set; } = default!;
        [ForeignKey(nameof(ParentCategoryId))] public Category? ParentCategory { get; set; }

        public ICollection<Category>? ChildrenCategories { get; set; }

        public ICollection<PostCategory>? PostCategories { get; set; }
    }
}