﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;

namespace BGTechTest.Web.API.Data.Repositories
{
    public interface IDataRepository
    {
        Task Save<T>(IList<T> Data) where T : class, new();
        Task<IList<T>> Read<T>() where T : class, new();
    }
}
