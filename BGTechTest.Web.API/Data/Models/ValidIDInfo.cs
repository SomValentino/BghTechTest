using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGTechTest.Web.API.Data.Models
{
    public class ValidIDInfo
    {
        public ValidIDInfo()
        {
            
        }
        public ValidIDInfo(string identityNumber, DateTime birthDate, 
            string gender, string cizitenship)
        {
            IdentityNumber = identityNumber;
            BirthDate = birthDate;
            Gender = gender;
            Cizitenship = cizitenship;
        }
        public string IdentityNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender  { get; set; }
        public string Cizitenship { get; set; }
    }
}
