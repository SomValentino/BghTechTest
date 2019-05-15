using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGTechTest.Web.API.Validation
{
    public interface IIdentityNumberValidator
    {
        int controlNumber { get; }
        IdentityNumberValidationResult Validate(string identityNumber);
    }
}
