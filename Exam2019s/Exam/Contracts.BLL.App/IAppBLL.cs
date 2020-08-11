﻿using System;
using Contracts.BLL.App.Services;
using ee.itcollege.aleksi.Contracts.BLL.Base;

namespace Contracts.BLL.App
{
    public interface IAppBLL : IBaseBLL
    {
        IHomeWorkService HomeWorks { get; }
        ISemesterService Semesters { get; }
        ISubjectService Subjects { get; }
        IStudentSubjectService StudentSubjects { get; }
        IStudentHomeWorkService StudentHomeWorks { get; }
    }
}