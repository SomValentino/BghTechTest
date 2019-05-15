using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using BGTechTest.Web.API.Validation;
using Microsoft.AspNetCore.Http;

namespace BGTechTest.Web.API.Data.Dtos
{
    public class CsvUploadDto
    {
        [Required]
        [CsvUploadFile("csv|txt",ErrorMessage = "Invalid file format or file too large")]
        public IFormFile CsvUploadFile { get; set; }
    }
}
