using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.BLL.Base;
using Contracts.DAL.Base;

namespace BLL.Base
{
    public class BaseBLL<TUnitOfWork> : IBaseBLL
        where TUnitOfWork : IBaseUnitOfWork
    {
        protected readonly TUnitOfWork UnitOfWork;

        private readonly Dictionary<Type, object> _serviceCache = new Dictionary<Type, object>();

        public BaseBLL(TUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<int> SaveChangesAsync() => await UnitOfWork.SaveChangesAsync();

        public int SaveChanges() => UnitOfWork.SaveChanges();

        public TService GetService<TService>(Func<TService> serviceCreationMethod)
        {
            if (_serviceCache.TryGetValue(typeof(TService), out var service))
            {
                return (TService) service;
            }

            service = serviceCreationMethod()!;
            
            _serviceCache.Add(typeof(TService), service);
            
            return (TService) service;
        }
    }
}