using System;
using System.Collections.Generic;
using System.Text;
using BGTechTest.Web.API.Service;
using BGTechTest.Web.API.Validation;

namespace BGTechTest.WebAPI.Tests.Helpers
{
    public class TestIdentityNumberValidator : IdentityNumberValidator
    {
        public TestIdentityNumberValidator(IIdentityNumberService identityNumberService) : base(identityNumberService)
        {
        }
    }
}
