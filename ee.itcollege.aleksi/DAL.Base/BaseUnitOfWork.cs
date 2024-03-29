﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base;

namespace ee.itcollege.aleksi.DAL.Base
{
    public abstract class BaseUnitOfWork: IBaseUnitOfWork
    {
        private readonly Dictionary<Type, object> _repoCache = new Dictionary<Type, object>();

        // Factory method
        protected TRepository GetRepository<TRepository>(Func<TRepository> repoCreationMethod)
        {
            if (_repoCache.TryGetValue(typeof(TRepository), out var repo))
            {
                return (TRepository) repo;
            }

            repo = repoCreationMethod()!;
            _repoCache.Add(typeof(TRepository), repo);
            return (TRepository) repo;
        }

        public abstract int SaveChanges();

        public abstract Task<int> SaveChangesAsync();
    }
}