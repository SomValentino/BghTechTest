using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BGTechTest.Web.API.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CsvUploadFileAttribute : ValidationAttribute
    {
        private List<string> AllowedExtensions { get; set; }
        public CsvUploadFileAttribute(string fileExtensions)
        {
            AllowedExtensions = fileExtensions.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override bool IsValid(object value)
        {
            if (value is IFormFile file)
            {
                var filename = file.FileName;
                var afileTYpe = AllowedExtensions.Any(y => filename.EndsWith(y));
                if (file.Length <= 5120000 && afileTYpe)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
