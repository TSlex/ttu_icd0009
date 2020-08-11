using System;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.DAL.Base;

namespace Domain
{
    public class Template: DomainEntityBaseMetaSoftUpdateDelete
    {
        public string TestValue { get; set; } = default!;

        public DateTime TestDate { get; set; } = default!;
    }
}