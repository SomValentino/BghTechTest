using System;
using System.Collections.Generic;
using System.Text;
using BGTechTest.Web.API.Service;
using Microsoft.Extensions.Configuration;

namespace BGTechTest.WebAPI.Tests.Helpers
{
    public class TestIdentityNumberService : IdentityNumberService
    {
        public TestIdentityNumberService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
