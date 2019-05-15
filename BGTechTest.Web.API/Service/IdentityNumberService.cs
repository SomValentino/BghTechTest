using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;
using BGTechTest.Web.API.Validation;

namespace BGTechTest.Web.API.Service
{
    public class IdentityNumberService : IIdentityNumberService
    {
        public IdInfo ExtractIdInformation(string[] idNumbers,IIdentityNumberValidator identityNumberValidator)
        {
            var idInfo = new IdInfo();
            foreach (var idnum in idNumbers)
            {
                // validate the identity number
                var result = identityNumberValidator.Validate(idnum);
                if (result.Isvalid)
                {
                    ValidIDInfo validIdInfo = ExtractValidIdInformation(idnum);
                    idInfo.validIdInfos.Add(validIdInfo);
                }
                else
                {
                    var invalidIdInfo = new InvalidIDInfo(idnum, result.ErrorMessage);
                    idInfo.InvalidIdInfos.Add(invalidIdInfo);
                }
            }

            return idInfo;
        }
        private ValidIDInfo ExtractValidIdInformation(string idnum)
        {
            DateTime dob = ExtractDoBFromIdentityNumber(idnum);
            string gender = ExtractGenderFromIdentityNumber(idnum);
            string citizenShip = ExtractCitizenShipFromIdentityNumber(idnum);
            return new ValidIDInfo(idnum,dob,gender,citizenShip);
        }

        private string ExtractCitizenShipFromIdentityNumber(string idnum)
        {
            int citizenNum = int.Parse(idnum[10].ToString());
            if (citizenNum == 0)
                return "SA Citizen";
            return "Non-SA Citizen";
        }

        private string ExtractGenderFromIdentityNumber(string idnum)
        {
            int genderNum = int.Parse(idnum[6].ToString());
            if (genderNum >= 0 && genderNum <= 4)
                return "Female";
            return "Male";
        }

        private DateTime ExtractDoBFromIdentityNumber(string idnum)
        {
            // the first six numbers of a valid id represents the Date of birth
            string DOB = idnum.Substring(0, 6);
            string year = DOB.Substring(0, 2);
            string month = DOB.Substring(2, 2);
            string day = DOB.Substring(4, 2);
            int yearNum = int.Parse(year);
            int currentYear = DateTime.Now.Year % 100; // get the last two digits
            string yearPrefix = "19";
            if (yearNum < currentYear)
            {
                yearPrefix = "20";
            }

            string cYear = yearPrefix + year;
            yearNum = int.Parse(cYear);
            int monthNum = int.Parse(month.TrimStart('0'));
            int dayNum = int.Parse(day.TrimStart('0'));

            DateTime DateOfBirth = new DateTime(yearNum, monthNum, dayNum);
            return DateOfBirth;
        }
    }
}
