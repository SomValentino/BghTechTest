using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;
using BGTechTest.Web.API.Validation;

namespace BGTechTest.Web.API.Service
{
    public interface IIdentityNumberService
    {
        IdInfo ExtractIdInformation(string[] idNumbers, IIdentityNumberValidator identityNumberValidator);
        DateTime ExtractDoBFromIdentityNumber(string idnum);
    }
}
