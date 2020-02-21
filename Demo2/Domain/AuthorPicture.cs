using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.DAL.Base;
using DAL.Base;

namespace Domain
{
    public class AuthorPicture : DomainEntityMetadata
    {
        [MaxLength(255)] public string PictureUrl { get; set; } = default!;

        [MaxLength(36)] public string AuthorId { get; set; } = default!;
        [ForeignKey(nameof(AuthorId))]
        public Author? Author { get; set; }
    }
}