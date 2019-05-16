using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;
using BGTechTest.Web.API.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace BGTechTest.Web.API.Data.Repositories
{
    public class Csvrepository : IDataRepository
    {
        private readonly IDataSerializer _dataSerializer;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Csvrepository(IDataSerializer dataSerializer,IHostingEnvironment hostingEnvironment)
        {
            _dataSerializer = dataSerializer;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task Save<T>(IList<T> Data, FileCsvType fileCsvType) where T : class, new()
        {
            string path = GetPath(fileCsvType);
            var fileStream = new FileStream(path,FileMode.Append,FileAccess.Write);
            await _dataSerializer.Serialize<T>(fileStream, Data);
        }

        private string GetPath(FileCsvType fileCsvType)
        {
            string path;
            switch (fileCsvType)
            {
                case FileCsvType.ValidIdFile:
                    path = Path.Combine(_hostingEnvironment.WebRootPath, @"Csv_ValidIdFile.txt");
                    break;
                case FileCsvType.InValidIdFile:
                    path = Path.Combine(_hostingEnvironment.WebRootPath, @"Csv_InValidIdFile.txt");
                    break;
                default:
                    throw new Exception("File type does not exist");
            }

            return path;
        }

        public Task<IList<T>> Read<T>(FileCsvType fileCsvType) where T : class, new()
        {
            string path = GetPath(fileCsvType);
            var fileStream = new FileStream(path,FileMode.Open,FileAccess.Read);
            return _dataSerializer.Deserialize<T>(fileStream);
        }
    }
}
