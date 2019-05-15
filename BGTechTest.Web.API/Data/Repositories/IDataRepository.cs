using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;

namespace BGTechTest.Web.API.Data.Repositories
{
    public interface IDataRepository
    {
        Task Save<T>(IEnumerable<T> Data,FileCsvType fileCsvType) where T : class, new();
        Task<IEnumerable<T>> Read<T>(FileCsvType fileCsvType) where T : class, new();
    }
}
