using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BGTechTest.Web.API.Service;
using BGTechTest.Web.API.Validation;
using BGTechTest.WebAPI.Tests.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BGTechTest.WebAPI.Tests
{
    [TestFixture()]
    public class IdentityNumberServiceTests
    {
        private IIdentityNumberValidator _identityNumberValidator;
        private IIdentityNumberService _identityNumberService;

        [SetUp]
        public void SetUp()
        {
            _identityNumberValidator = new TestIdentityNumberValidator();
            _identityNumberService = new TestIdentityNumberService();
        }

        [Test]
        public void ExtractIdInformation_GivenRightAndWrongIdentityNumber_SplitsIdsToValidAndInvalidIdInfo()
        {
            var idNumbers = new string[] {"8605065397083", "0109046424188"};
            var idInfo = _identityNumberService.ExtractIdInformation(idNumbers, _identityNumberValidator);

            Assert.That(idInfo.validIdInfos.Any(),Is.True);
            Assert.That(idInfo.InvalidIdInfos.Any,Is.True);
        }

        [Test]
        public void ExtractIdInformation_GivenAValidId_ReturnsValidIdProperties()
        {
            var idNumbers = new string[] { "8605065397083" };
            var idInfo = _identityNumberService.ExtractIdInformation(idNumbers, _identityNumberValidator);

            Assert.That(idInfo.validIdInfos[0].IdentityNumber, Is.EqualTo("8605065397083"));
            Assert.That(idInfo.validIdInfos[0].BirthDate,Is.EqualTo(new DateTime(1986,5,6)));
            Assert.That(idInfo.validIdInfos[0].Gender,Is.EqualTo("Male"));
            Assert.That(idInfo.validIdInfos[0].Cizitenship,Is.EqualTo("SA Citizen"));
            Assert.That(idInfo.InvalidIdInfos.Any(),Is.False);
        }

        [Test]
        public void ExtractIdInformation_GivenAnInvalidId_ReturnsInvalidIdProperties()
        {
            var idNumbers = new string[] { "0109046424188" };
            var idInfo = _identityNumberService.ExtractIdInformation(idNumbers, _identityNumberValidator);

            Assert.That(idInfo.InvalidIdInfos[0].IdentityNumber,Is.EqualTo("0109046424188"));
            Assert.That(idInfo.InvalidIdInfos[0].ReasonsFailed,Is.EqualTo("The invalid control character. Expected: 8 but got : 9"));
            Assert.That(idInfo.validIdInfos.Any(),Is.False);
        }
    }
}
