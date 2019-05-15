using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGTechTest.Web.API.Data.Models
{
    public class IdInfo
    {
        public IdInfo()
        {
            this.validIdInfos = new List<ValidIDInfo>();
            InvalidIdInfos = new List<InvalidIDInfo>();
        }
        public List<ValidIDInfo> validIdInfos { get; set; }
        public List<InvalidIDInfo> InvalidIdInfos { get; set; }
    }
}
