using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace BGTechTest.Web.API.Data.Models
{
    public enum FileCsvType
    {
        ValidIdFile,
        InValidIdFile
    }
    public class InvalidIDInfo
    {
        public InvalidIDInfo()
        {
            
        }
        public InvalidIDInfo(string identityNumber, string reasonsFailed)
        {
            IdentityNumber = identityNumber;
            ReasonsFailed = reasonsFailed;
        }
        public string IdentityNumber { get; set; }
        public string ReasonsFailed { get; set; }
    }
}
