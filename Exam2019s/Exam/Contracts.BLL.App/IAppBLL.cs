﻿using System;
using Contracts.BLL.App.Services;
using ee.itcollege.aleksi.Contracts.BLL.Base;

namespace Contracts.BLL.App
{
    public interface IAppBLL : IBaseBLL
    {
        ITemplateService Templates { get; }
    }
}