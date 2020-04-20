using System;
using BLL.App.Services;
using BLL.Base;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        public AppBLL(IAppUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IProfileService Profiles => GetService<IProfileService>(() => new ProfileService(UnitOfWork));
        public IPostService Posts => GetService<IPostService>(() => new PostService(UnitOfWork));
    }
}