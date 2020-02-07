using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Contact
    {
        public int ContactId { get; set; }
        
        [MaxLength(256)]
        public string ContactValue { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
        
        public int ContactTypeId { get; set; }
        public ContactType ContactType { get; set; }
    }
}