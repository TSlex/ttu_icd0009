using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.App.DTO
{
    public class Feed
    {   
        [Display(Name = nameof(Posts), ResourceType = typeof(Resourses.BLL.App.DTO.Feed.Feed))]
        public ICollection<Post>? Posts { get; set; }
    }
}