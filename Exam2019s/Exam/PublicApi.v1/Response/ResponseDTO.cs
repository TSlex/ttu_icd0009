using System.Collections.Generic;
using System.Linq;

namespace PublicApi.v1.Response
{
    public class ResponseDTO
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // ReSharper disable once CollectionNeverQueried.Local
        public IList<string> Messages { get; set; } = new List<string>();
        
        public ResponseDTO()
        {
        }
        
        public ResponseDTO(params string[] messages)
        {
            foreach (var message in messages)
            {
                Messages.Add(message);
            }
        }
    }
}