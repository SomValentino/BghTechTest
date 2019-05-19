using System;
using System.Collections.Generic;
using System.Text;
using BGTechTest.Web.API.Data.Repositories;
using BGTechTest.Web.API.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BGTechTest.WebAPI.Tests.Helpers
{
    public class TestCsvRepository : CsvRepository
    {
        public TestCsvRepository(IDataSerializer dataSerializer, IHostingEnvironment hostingEnvironment,IConfiguration configuration) : 
            base(dataSerializer, hostingEnvironment,configuration)
        {
        }
    }
}
