using Contracts.DAL.Base;
using DAL.Base;

namespace BLL.App.DTO.Base
{
    public class DomainEntityBaseMetaSoftDelete: DomainEntityBaseMetadata, ISoftDeleteEntity
    {
    }
}