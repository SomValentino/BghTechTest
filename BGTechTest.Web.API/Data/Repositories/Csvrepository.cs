using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;
using BGTechTest.Web.API.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

namespace BGTechTest.Web.API.Data.Repositories
{
    public class CsvRepository : IDataRepository
    {
        private readonly IDataSerializer _dataSerializer;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public CsvRepository(IDataSerializer dataSerializer,IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _dataSerializer = dataSerializer;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task Save<T>(IList<T> Data) where T : class, new()
        {
            string path = GetPath<T>();
            var fileStream = new FileStream(path,FileMode.Append,FileAccess.Write);
            await _dataSerializer.Serialize<T>(fileStream, Data);
        }

        private string GetPath<T>() where T: class,new()
        {
            string path;
            if (typeof(T) == typeof(ValidIDInfo))
            {
                path = Path.Combine(_hostingEnvironment.WebRootPath, _configuration.GetSection("AppConfig:ValidIdOutputFile").Value);
            }
            else if (typeof(T) == typeof(InvalidIDInfo))
            {
                path = Path.Combine(_hostingEnvironment.WebRootPath, _configuration.GetSection("AppConfig:InvalidIdOutputFile").Value);
            }
            else
            {
                throw new Exception("File type does not exist");
            }
            return path;
        }

        public async Task<IList<T>> Read<T>() where T : class, new()
        {
            string path = GetPath<T>();
            if(!File.Exists(path))
                return new List<T>();
            var fileStream = new FileStream(path,FileMode.Open,FileAccess.Read);
            return await _dataSerializer.Deserialize<T>(fileStream);
        }
    }
}
