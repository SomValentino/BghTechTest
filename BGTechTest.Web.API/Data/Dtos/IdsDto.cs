using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BGTechTest.Web.API.Data.Dtos
{
    public class IdsDto
    {
        [Required]
        public string idNumbers { get; set; }
    }
}
