using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BGTechTest.WebAPI.Tests.Helpers
{
    public class ConfigurationSetup
    {
        public static IConfiguration SetUpConfiguration()
        {
            return new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                {"AppConfig:ValidIdOutputFile", "Csv_ValidIdFile.txt"},
                {"AppConfig:InvalidIdOutputFile","Csv_InValidIdFile.txt" },
                {"AppConfig:SACitizen","SA Citizen"},
                {"AppConfig:OtherCitizen","Non-SA Citizen" },
                {"AppConfig:Male","Male" },
                {"AppConfig:Female","Female"}
            }).Build();
        }
        
    }
}
