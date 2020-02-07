using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ContactType
    {
        public int ContactTypeId { get; set; }
        
        [MaxLength(256)]
        public string ContactTypeValue { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}