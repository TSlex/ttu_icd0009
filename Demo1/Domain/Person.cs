using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Person
    {
        public int PersonId { get; set; }
        
        [MaxLength(256)]
        public string FirstName { get; set; }
        
        [MaxLength(256)]
        public string LastName { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}