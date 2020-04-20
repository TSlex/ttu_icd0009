using System;
using BLL.App.DTO;
using BLL.Base.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class ProfileService : BaseService, IProfileService
    {
        public ProfileService(IAppUnitOfWork uow) : base()
        {
        }
        
        public void GetProfileFull()
        {
            throw new System.NotImplementedException();
        }
    }
}