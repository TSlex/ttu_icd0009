using System.Collections.Generic;

namespace BLL.App.DTO
{
    public class Feed
    {
        public ICollection<Post>? Posts { get; set; }
    }
}