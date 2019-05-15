using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace BGTechTest.Web.API.Helpers
{
    public interface IDataSerializer
    {
        Task Serialize<T>(Stream stream, IEnumerable<T> data) where T : class,new();
        Task<IEnumerable<T>> Deserialize<T>(Stream stream) where T : class, new();
        string CsvData { get; }
        
    }
}
