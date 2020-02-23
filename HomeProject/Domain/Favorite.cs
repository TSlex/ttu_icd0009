using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Favorite: DomainEntityMetadata
    {
        [MaxLength(36)] public string ProfileId { get; set; } = default!;
        public Profile? Profile { get; set; }
        
        [MaxLength(36)] public string PostId { get; set; } = default!;
        public Post? Post { get; set; }
    }
}