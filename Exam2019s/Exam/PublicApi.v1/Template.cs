using System;
using ee.itcollege.aleksi.DAL.Base;

namespace PublicApi.v1
{
    public class Template : DomainEntityBaseMetaSoftUpdateDelete
    {
        public string TestValue { get; set; } = default!;
        public DateTime TestDate { get; set; } = default!;
    }
}