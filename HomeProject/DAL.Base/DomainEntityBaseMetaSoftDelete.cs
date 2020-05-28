using System;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public class DomainEntityBaseMetaSoftDelete: DomainEntityBaseMetadata, ISoftDeleteEntity
    {
    }
}