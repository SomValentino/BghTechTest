using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;
using BGTechTest.Web.API.Helpers;
using BGTechTest.WebAPI.Tests.Helpers;
using BGTechTest.WebAPI.Tests.Resources;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BGTechTest.WebAPI.Tests
{
    [TestFixture()]
    public class CsvSerializerTests
    {
        private IDataSerializer _dataSerializer;
        private List<ValidIDInfo> _idInfos;
        private string _idFiledata;
        [SetUp]
        public void Setup()
        {
            _dataSerializer = new TestDataSerializer();
            _idInfos = new List<ValidIDInfo>
            {
                new ValidIDInfo("8709046424188",new DateTime(1987,9,4),"Non-SA Citizen","Male"),
                new ValidIDInfo("8605065397083",new DateTime(1986,5,6),"SA Citizen","Male"),
            };
            _idFiledata = IDTestFile.IdInfo;
        }

        [Test]
        public async Task Serialize_GivenStreamAndIdInfo_SavesCsvToValidIdFile()
        {
            var filestream = new FileStream("ValidCsv.txt",FileMode.Create);
            await _dataSerializer.Serialize(filestream, _idInfos);
            var actualValue ="8709046424188,1987/09/04,Non-SA Citizen,Male\r\n" +
                                "8605065397083,1986/05/06,SA Citizen,Male\r\n";
            Assert.That(actualValue,Is.EqualTo(_dataSerializer.CsvData));
        }

        [Test]
        public async Task Deserialize_GivenStream_ReturnsAListOfIdInfo()
        {
            var byteArray = Encoding.UTF8.GetBytes(_idFiledata);
            var stream = new MemoryStream(byteArray);

            var result = (await _dataSerializer.Deserialize<ValidIDInfo>(stream)).ToList();
            Assert.That(JsonConvert.SerializeObject(result),Is.EqualTo(JsonConvert.SerializeObject(_idInfos)));
        }
    }

}
