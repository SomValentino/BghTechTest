using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BGTechTest.Web.API.Controllers;
using BGTechTest.Web.API.Data.Dtos;
using BGTechTest.Web.API.Data.Models;
using BGTechTest.Web.API.Data.Repositories;
using BGTechTest.Web.API.Service;
using BGTechTest.Web.API.Validation;
using BGTechTest.WebAPI.Tests.Helpers;
using BGTechTest.WebAPI.Tests.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NLog.Web.LayoutRenderers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BGTechTest.WebAPI.Tests
{

    [TestFixture()]
    public class IdentityNumberControllerTests
    {
        private Mock<IDataRepository> _dataRepository;
        private Mock<IHostingEnvironment> _hostingEnvironment;
        private Mock<IFormFile> _formFile;
        private Mock<ILogger<IdentityNumberController>> _logger;
        private IIdentityNumberService _identityNumberService;
        private IIdentityNumberValidator _identityNumberValidator;
        private IdentityNumberController _identityNumberController;

        [SetUp]
        public void SetUp()
        {

            _dataRepository = new Mock<IDataRepository>();
            //_dataRepository.Setup(x => x.Save(It.IsAny<List<ValidIDInfo>>(), It.IsAny<FileCsvType>()));
            _hostingEnvironment = new Mock<IHostingEnvironment>();
            _formFile = new Mock<IFormFile>();
            _logger = new Mock<ILogger<IdentityNumberController>>();
            _identityNumberValidator = new TestIdentityNumberValidator();
            _identityNumberService = new TestIdentityNumberService();
            _identityNumberController = new IdentityNumberController(_dataRepository.Object,_identityNumberValidator,
                _identityNumberService,_hostingEnvironment.Object,_logger.Object);
            SetUpDataRepository();
        }

        private void SetUpDataRepository()
        {
            _dataRepository.Setup(x => x.Read<ValidIDInfo>(It.IsAny<FileCsvType>()))
                .Returns(Task.FromResult<IList<ValidIDInfo>>(new List<ValidIDInfo>
                {
                    new ValidIDInfo("8709046424188", new DateTime(1987, 9, 4), "Non-SA Citizen", "Male"),
                    new ValidIDInfo("8605065397083", new DateTime(1986, 5, 6), "SA Citizen", "Male"),
                }));
            _dataRepository.Setup(x => x.Read<InvalidIDInfo>(It.IsAny<FileCsvType>()))
                .Returns(Task.FromResult<IList<InvalidIDInfo>>(new List<InvalidIDInfo>
                {
                    new InvalidIDInfo("8709046424188", ""),
                    new InvalidIDInfo("8605065397083", "Male"),
                }));
        }

        [Test]
        public async Task AddIdentityNumber_GivenValidAndInvalidId_EnsureThatOutputsIsSavedToCsv()
        {
            var iddto = new IdsDto {idNumbers = "8605065447083\r\n8605065397083\r\n"};
            var result = await _identityNumberController.AddIdentityNumber(iddto);

            _dataRepository.Verify(x => 
                x.Save(It.IsAny<List<ValidIDInfo>>(),It.IsAny<FileCsvType>()),Times.AtLeastOnce);
            Assert.That(result,Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task AddIdentityNumber_GivenAnEmptyStringAsIds_ReturnsABadRequest()
        {
            var iddto = new IdsDto { idNumbers = " " };
            var result = await _identityNumberController.AddIdentityNumber(iddto);

            _dataRepository.Verify(x =>
                x.Save(It.IsAny<List<ValidIDInfo>>(), It.IsAny<FileCsvType>()), Times.Never);
            Assert.That(result,Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UploadIdsFromCsv_GivenAnUploadFileOfValidAndInvalidIds_SavesToCsvAndReturnsNoContent()
        {
            _formFile.Setup(x => x.Length).Returns(312000); // ensure size is less than 5mb
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("8605065447083\r\n8605065397083\r\n"));
            _formFile.Setup(x => x.OpenReadStream()).Returns(memoryStream); //stream of data
            _formFile.Setup(x => x.FileName).Returns("ids.csv");// validation for upload file extension
            var csvUpload = new CsvUploadDto{CsvUploadFile = _formFile.Object};

            var result = await _identityNumberController.UploadIdsFromCsv(csvUpload);

            _dataRepository.Verify(x =>
                x.Save(It.IsAny<List<ValidIDInfo>>(), It.IsAny<FileCsvType>()), Times.AtLeastOnce);
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task UploadIdsFromCsv_GivenEmptyUploadFile_ReturnsBadRequest()
        {
            _formFile.Setup(x => x.Length).Returns(0); // ensure size is less than 5mb
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(""));
            _formFile.Setup(x => x.OpenReadStream()).Returns(memoryStream); //stream of data
            _formFile.Setup(x => x.FileName).Returns("ids.csv");
            var csvUpload = new CsvUploadDto { CsvUploadFile = _formFile.Object };

            var result = await _identityNumberController.UploadIdsFromCsv(csvUpload);

            _dataRepository.Verify(x =>
                x.Save(It.IsAny<List<ValidIDInfo>>(), It.IsAny<FileCsvType>()), Times.Never);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetIds_GivenCsvsForValidAndInvalidIdsExist_ReturnsInstanceOfIdInfo()
        {
            var result = await _identityNumberController.GetIds();
            var resultObject = result as OkObjectResult;
            var resultValue = resultObject.Value;
            var resultContent = resultValue as IdInfo;

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(resultValue,Is.InstanceOf<IdInfo>());
            Assert.That(resultContent.validIdInfos.Count,Is.EqualTo(2));
            Assert.That(resultContent.InvalidIdInfos.Count,Is.EqualTo(2));
        }
    }
}
