using System;
using ee.itcollege.aleksi.DAL.Base;

namespace DAL.App.DTO
{
    public class Template : DomainEntityBaseMetaSoftUpdateDelete
    {
        public string TestValue { get; set; } = default!;
        public DateTime TestDate { get; set; } = default!;
    }
}