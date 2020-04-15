﻿using System;
using BLL.Base;
using Contracts.BLL.App;
using Contracts.DAL.App;

namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        public AppBLL(IAppUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

//        public IAnimalService Animals =>
//            GetService<IAnimalService>(() => new AnimalService(UnitOfWork));
//
//        public IOwnerService Owners =>
//            GetService<IOwnerService>(() => new OwnerService(UnitOfWork));
//
//        public IOwnerAnimalService OwnerAnimals =>
//            GetService<IOwnerAnimalService>(() => new OwnerAnimalService(UnitOfWork));
    }
}