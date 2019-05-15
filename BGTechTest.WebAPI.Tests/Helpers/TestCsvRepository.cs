using System;
using System.Collections.Generic;
using System.Text;
using BGTechTest.Web.API.Data.Repositories;
using BGTechTest.Web.API.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace BGTechTest.WebAPI.Tests.Helpers
{
    public class TestCsvRepository : Csvrepository
    {
        public TestCsvRepository(IDataSerializer dataSerializer, IHostingEnvironment hostingEnvironment) : base(dataSerializer, hostingEnvironment)
        {
        }
    }
}
