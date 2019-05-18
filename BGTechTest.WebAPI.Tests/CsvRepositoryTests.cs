using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;
using BGTechTest.Web.API.Data.Repositories;
using BGTechTest.Web.API.Helpers;
using BGTechTest.WebAPI.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BGTechTest.WebAPI.Tests
{
    [TestFixture()]
    public class CsvRepositoryTests
    {
        private Mock<IHostingEnvironment> _hostingEnvironmentMock;
        private IDataSerializer _dataSerializer;
        private IDataRepository _dataRepository;
        private List<ValidIDInfo> _idInfos;
        private List<InvalidIDInfo> _invalidIdInfos;

        [SetUp]
        public void SetUp()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string path = System.IO.Path.GetDirectoryName(asm.Location);
            _hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            _hostingEnvironmentMock.Setup(c => c.WebRootPath).Returns(() => path);
            _dataSerializer = new TestDataSerializer();
            _dataRepository = new TestCsvRepository(_dataSerializer,_hostingEnvironmentMock.Object);
            _idInfos = new List<ValidIDInfo>
            {
                new ValidIDInfo("8709046424188",new DateTime(1987,9,4),"Non-SA Citizen","Male"),
                new ValidIDInfo("8605065397083",new DateTime(1986,5,6),"SA Citizen","Male"),
            };
            _invalidIdInfos = new List<InvalidIDInfo>
            {
                new InvalidIDInfo("870904642489","The identity number contains " +
                                                 "characters that are not numbers|The Identity number has " +
                                                 "less than 13 digits"),
                new InvalidIDInfo("870904642489001","The identity number contains " +
                                                 "characters that are not numbers|The Identity number has " +
                                                 "more than 13 digits"),
            };
        }

        [Test]
        public async Task Save_GivenDataWithValidIdInfo_SavesCsvToValidIdCsvFile()
        {
            await _dataRepository.Save(_idInfos);
            Assert.That(File.Exists("Csv_ValidIdFile.txt"),Is.True);
        }

        [Test]
        public async Task Save_GivenDataWithInvalidIdInfo_SaveCsvToInvalidIdCsvFile()
        {
            await _dataRepository.Save(_invalidIdInfos);
            Assert.That(File.Exists("Csv_InValidIdFile.txt"),Is.True);
        }

        [Test]
        public async Task Read_GivenStreamOfValidIdInfo_ReturnsAListValidIdInfo()
        {
            var result = await _dataRepository.Read<ValidIDInfo>();
            Assert.That(result,Is.Not.Null);
            Assert.That(result.Any(),Is.True);
        }
        [Test]
        public async Task Read_GivenStreamOfInvalidIdInfo_ReturnsAListInvalidIdInfo()
        {
            var result = await _dataRepository.Read<InvalidIDInfo>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Any(), Is.True);
        }
    }
}
