using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BGTechTest.Web.API.Validation
{

    public class IdentityNumberValidator : IIdentityNumberValidator
    {
        public int controlNumber { get; private set; }
        public IdentityNumberValidationResult Validate(string identityNumber)
        {
            List<string> errorReasons = new List<string>();
            var result = new IdentityNumberValidationResult();
            if (NullAndWhiteSpaceCheck(identityNumber, errorReasons))
            {
                result.Isvalid = false;
                result.ErrorMessage = string.Join("|", errorReasons);
                return result;
            }
               
            // check if all charcters are digits and the length
            if (NumberAndLengthCheck(identityNumber, errorReasons))
            {
                result.Isvalid = false;
                result.ErrorMessage = string.Join("|", errorReasons);
                return result;
            }
            // calculate control character
            ValidateControlCharacter(identityNumber, errorReasons);

            return new IdentityNumberValidationResult
            {
                Isvalid = !errorReasons.Any(),
                ErrorMessage = string.Join("|",errorReasons)
            };
        }

        private bool NullAndWhiteSpaceCheck(string identityNumber, List<string> errorReasons)
        {
            if (string.IsNullOrEmpty(identityNumber) || string.IsNullOrWhiteSpace(identityNumber))
            {
                errorReasons.Add("The identity number cannot be null, empty or whitespace");
            }

            return errorReasons.Any();
        }

        private void ValidateControlCharacter(string identityNumber, List<string> errorMessages)
        {
            int sumOdds = 0;
            int multEvens = 0;
            for (int i = 0; i<identityNumber.Length/2; i++)
            {
                //sum all numbers in the odd position
                sumOdds += int.Parse(identityNumber[2 * i].ToString());

            }

            for (int i = 0; i < identityNumber.Length / 2; i++)
            {
                //concatenate all numbers in the even position
                
                multEvens = multEvens*10 + int.Parse(identityNumber[2 * i + 1].ToString());
            }

            multEvens *= 2;

            int sumMultEvens = 0;

            do
            {
                sumMultEvens += multEvens % 10;
                multEvens = multEvens / 10;
            } while (multEvens > 0);

            int total = sumMultEvens + sumOdds;
            int controlNum = 10 - (total % 10);
            int actualControlNum = int.Parse(identityNumber[identityNumber.Length - 1].ToString());
            if (controlNum != actualControlNum)
            {
                errorMessages.Add($"The invalid control character. Expected: {actualControlNum}, but got : {controlNum}");
            }
                
            controlNumber = controlNum;

        }

        private bool NumberAndLengthCheck(string identityNumber, List<string> errorReasons)
        {
            if (!Regex.IsMatch(identityNumber, @"^\d+$"))
            {
                errorReasons.Add("The identity number contains characters that are not numbers");
            }

            // check for length of digits
            if (identityNumber.Length < 13)
            {
                errorReasons.Add("The Identity number has less than 13 digits");
            }

            if (identityNumber.Length > 13)
            {
                errorReasons.Add("The Identity number has more than 13 digits");
            }

            return errorReasons.Any();
        }
    }
}
